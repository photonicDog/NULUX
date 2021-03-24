using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class SongManager : SerializedMonoBehaviour {
    // TODO: Organize this PLEASE
    public static SongManager Instance;
    
    public Track currentTrack;
    public List<LineDataCommand> lcList;

    private Note[] heldNotes;
    private Queue<ChartDialogueCommand> dcQueue;

    [SerializeField] private GameObject introAnimation = default;
    [SerializeField] private AudioSource introAnimationSound = default;
    
    [SerializeField] private GameObject mechanicObject = default;
    private NOVAMechanic mechanic;

    public float score = 0;
    public TextMeshProUGUI scoreboard;
    [SerializeField] private SongUIManager ui;
    public DialogueTalkspriter Talkspriter;
    private ResultsPanel rp;
    [HideInInspector] public Dictionary<ScoringHeuristic, int> noteResults;

    [SerializeReference] private DialogueRunner dialogueRunner = default;
    [SerializeReference] private Button dialogueAdvanceButton = default;

    private float perfectNoteScore;

    public bool isStoryMode;

    public float perfectWindow = 20;
    public float greatWindow = 40;
    public float goodWindow = 90;

    public Image driveGauge;
    private float drive = 0f;
    private float pacemaker = 0f;

    private List<Note> chartNotes;
    private int origNoteCt;

    [SerializeField] private OffsetBar ob = default;
    [HideInInspector] public List<HitData> recordedData;

    public int combo;
    private float endBeat;

    public Image endFade;

    void Awake() {
        Instance = this;
        StartCoroutine(WaitForTrackSignal());
    }

    private void OnDestroy() {
        Instance = null;
    }

    public void SetCurrentTrack(Track t) {
        currentTrack = t;
        isStoryMode = t.isStoryModeTrack;
    }

    IEnumerator WaitForTrackSignal() {
        while (currentTrack == null) {
            yield return new WaitForFixedUpdate();
        }
        InputSystem.pollingFrequency = 4000f;
        chartNotes = new List<Note>();
        lcList = new List<LineDataCommand>();
        heldNotes = new Note[4];
        rp = GetComponent<ResultsPanel>();
        mechanic = mechanicObject.GetComponent<NOVAMechanic>();
        mechanic.BuildMechanic(ComboManager, ScoreManager, RecordDataManager);
        
        recordedData = new List<HitData>();
        noteResults = new Dictionary<ScoringHeuristic, int>() {
            {ScoringHeuristic.STELLAR, 0},
            {ScoringHeuristic.PERFECT, 0},
            {ScoringHeuristic.GREAT, 0},
            {ScoringHeuristic.GOOD, 0},
            {ScoringHeuristic.MISS, 0}
        };
        
        Build();
    }

    void Build() {
        for (int i = 0; i < 4; i++) {
            heldNotes[i] = null;
        }

        ReadMIDI(Application.streamingAssetsPath + "/" + currentTrack.PathToChart +".mid", currentTrack);
        Conductor.Instance.SetSong(currentTrack.Audio);
        origNoteCt = chartNotes.Count;
        
        perfectNoteScore = 1000000f / ((chartNotes.Count));
        if (currentTrack.yp) dialogueRunner.Add(currentTrack.yp);
        driveGauge.fillAmount = drive;
        
        ui.UpdateSongAttributes(currentTrack.trackName);

        // Build all necessary components and then turn em off until they're ready.
        

        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence() {
        introAnimation.SetActive(true);
        introAnimationSound.Play();
        Conductor.Instance.PrimeSong(8f);
        yield return new WaitForSeconds(8f);
        introAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
        if (Conductor.Instance.playing && dcQueue.Count > 0) {
            // Chart dialogue
            if (dcQueue.Peek().timing < Conductor.Instance.GetSongTime()) {
                ChartDialogueCommand dcm = dcQueue.Dequeue();
                Debug.Log("Running command " + dcm.command[0]);
                switch (dcm.command[0]) {
                    case "PLAY":
                        RunDialogueNode(dcm.command[1]);
                        break;
                    case "ADV":
                        AdvanceDialogueNode();
                        break;
                }
            }
        }
    }

    private void RunDialogueNode(string node) {
        dialogueRunner.StartDialogue(node);
    }

    private void AdvanceDialogueNode() {
        dialogueAdvanceButton.onClick.Invoke();
    }

    private void LateUpdate() {
        if (Conductor.Instance.playing) {
            if (Conductor.Instance.GetSongTime() >= endBeat) {
                Conductor.Instance.StopSong();
                Debug.Log("End beat reached.");
                EndSong();
            }
        }
    }


    void EndSong() {
        StartCoroutine(FadeOut(120f));
    }

    IEnumerator FadeOut(float timeInFrames) {
        while (endFade.color.a < 0.99) {
            endFade.color += new Color(0,0,0,1/timeInFrames);
            yield return new WaitForEndOfFrame();
        }
        
        mechanicObject.SetActive(false);
        rp.endBeat = endBeat;
        rp.SetScore((int)score);
    }

    // Read midi from path.
    void ReadMIDI(string midiPath, Track track) {
        TrackLines asscTrack = track.Lines;
        MidiFile mf = new MidiFile(midiPath, false);
        dcQueue = new Queue<ChartDialogueCommand>();

        // Tempo track
        foreach (var midiEvent in mf.Events[0]) {
            if (midiEvent is TempoEvent) {
                // TODO: Make event queue for BPM changes and stuff.
                Conductor.Instance.SetBPM((float)(midiEvent as TempoEvent).Tempo);
            } else if (midiEvent is TextEvent) {
                // TODO: Add json-based animation or whatever here.
                if ((midiEvent as TextEvent).MetaEventType == MetaEventType.TextEvent) 
                    Conductor.Instance.SetSongOffset(float.Parse((midiEvent as TextEvent).Text));
            }
        }
        
        // Note track
        foreach (var midiEvent in mf.Events[1]) {
            if (MidiEvent.IsNoteOn(midiEvent)) {
                int length = ((NoteOnEvent) midiEvent).NoteLength;

                bool hold = length > 1;
                if (!hold) {
                    chartNotes.Add(new Note(
                        (float) midiEvent.AbsoluteTime / 480,
                        ((NoteOnEvent) midiEvent).NoteNumber, ((NoteOnEvent) midiEvent).Velocity));
                }
                else {
                    chartNotes.Add(new Note(
                        (float) midiEvent.AbsoluteTime / 480,
                        ((NoteOnEvent) midiEvent).NoteNumber,
                        ((NoteOnEvent) midiEvent).Velocity,
                        length / 480f));
                }
            }
        }

        //LineCMD track
        // TODO: FIX THIS
        foreach (var midiEvent in mf.Events[2]) {
            if (MidiEvent.IsNoteOn(midiEvent)) {
                NoteOnEvent nev = midiEvent as NoteOnEvent;
                lcList.Add(new LineDataCommand(
                    asscTrack.trackLines[nev.NoteNumber], 
                    nev.AbsoluteTime/480f, 
                    nev.AbsoluteTime/480f + (nev.NoteLength-1)/480f)
                );
            }
        }
        
        mechanic.ImportNotes(chartNotes, lcList);

        if (currentTrack.yp != null)
        foreach (var midiEvent in mf.Events[3]) {
            if (midiEvent is TextEvent && ((TextEvent) midiEvent).MetaEventType == MetaEventType.Marker) {
                TextEvent tev = midiEvent as TextEvent;
                string[] cmds = tev.Text.Split(' ');
                dcQueue.Enqueue(new ChartDialogueCommand(cmds, (float) midiEvent.AbsoluteTime / 480));
            }
        }

        //End track
        foreach (var midiEvent in mf.Events[4]) {
            if (MidiEvent.IsNoteOn(midiEvent)) {
                endBeat = ((NoteOnEvent) midiEvent).AbsoluteTime / 480f;
                break;
            }
        }
    }

    private void ResetTrack() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Input method.
    public void SongKey(InputAction.CallbackContext context) {        
        mechanic.QueryInput(context);
    }

    public void AlterOffset(InputAction.CallbackContext context) {
        Conductor.Instance.offset += context.ReadValue<float>();
    }

    public void ResetButton(InputAction.CallbackContext context) {
        if (context.started) {
            ResetTrack();
        }
    }

    private void ComboManager(int add) {
        if (add == 0) combo = 0;
        else combo += add;

        ui.UpdateCombo(combo);
    }

    private void ScoreManager(ScoringJudgement judgement) {
        float add = 0;
        float diff = 1f/(origNoteCt);
        pacemaker += diff * 0.76f;

        add = judgement.scoreMultiplier;
        drive += diff * judgement.clearMultiplier;
        noteResults[judgement.heur]++;
        
        score += add*perfectNoteScore;

        drive = Mathf.Clamp01(drive);
        pacemaker = Mathf.Clamp01(pacemaker);
        
        // TODO: Add to UI
        ui.UpdateScore((int)score);
        ui.UpdateDrive(drive, pacemaker);
    }

    private void RecordDataManager(HitData data) {
        recordedData.Add(data);
        ob.MakeMark(data.offset, data.heur);
    }
    
    public void ReturnToMenu() {
        if (SceneManager.GetActiveScene().buildIndex == 4)
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
    }
}

class ChartDialogueCommand {
    public string[] command;
    public float timing;

    public ChartDialogueCommand(string[] command, float timing) {
        this.command = command;
        this.timing = timing;
    }
}