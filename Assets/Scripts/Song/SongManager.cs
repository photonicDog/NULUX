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
    // Start is called before the first frame update
    public Track currentTrack;
    public List<LineDataCommand> lcList;

    private Note[] heldNotes;
    private Queue<ChartDialogueCommand> dcQueue;

    public Vector3 MechanicSpawn;
    [SerializeField] private GameObject introAnimation = default;
    [SerializeField] private AudioSource introAnimationSound = default;


    private Dictionary<string, NoteType> controlMapping;
    
    [SerializeField] private GameObject mechanicObject = default;
    private NOVAMechanic mechanic;

    public float score = 0;
    public TextMeshProUGUI scoreboard;
    private ResultsPanel rp;

    [SerializeReference] private DialogueRunner dialogueRunner = default;
    [SerializeReference] private Button dialogueAdvanceButton = default;
    [SerializeField] private DialogueTalkspriter talkspriter = default;

    private float perfectNoteScore;

    public bool isStoryMode;

    public float perfectWindow = 20;
    public float greatWindow = 40;
    public float goodWindow = 90;

    public Image driveGauge;
    private float drive = 0.5f;

    private List<Note> chartNotes;
    private float _temporalWindow;

    [SerializeField] private OffsetBar ob = default;

    public int combo;
    private float endBeat;

    public Image endFade;
    
    private float mTiming = -1;

    void Start() {
        StartCoroutine(WaitForTrackSignal());
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
        mechanic.BuildMechanic(ComboManager, ScoreManager, OffsetBarManager);
        Build();
    }

    void Build() {
        for (int i = 0; i < 4; i++) {
            heldNotes[i] = null;
        }

        controlMapping = new Dictionary<string, NoteType>();

        ReadMIDI(Application.streamingAssetsPath + "\\"+ currentTrack.PathToChart +".mid", currentTrack.Lines);
        Conductor.Instance.SetSong(currentTrack.Audio);

        int noteCt = 0;

        perfectNoteScore = 1000000f / ((chartNotes.Count));
        if (currentTrack.yp) dialogueRunner.Add(currentTrack.yp);
        driveGauge.fillAmount = drive;
        
        ControlMapper();

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
        _temporalWindow = 60f / Conductor.Instance.bpm;

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
        rp.SetScore((int)score);
    }

    // Read midi from path.
    void ReadMIDI(string midiPath, TrackLines asscTrack) {
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
                    nev.AbsoluteTime/480f + nev.NoteLength/480f)
                );
            }
        }
        
        mechanic.ImportNotes(chartNotes, lcList);

        if (currentTrack.yp != null)
        foreach (var midiEvent in mf.Events[3]) {
            if (midiEvent is TextEvent && ((TextEvent) midiEvent).MetaEventType == MetaEventType.Marker) {
                TextEvent tev = midiEvent as TextEvent;
                string[] cmds = tev.Text.Split(',');
                dcQueue.Enqueue(new ChartDialogueCommand(cmds, (float) midiEvent.AbsoluteTime / 480));
            }
        }

        //End track
        foreach (var midiEvent in mf.Events[4]) {
            if (MidiEvent.IsNoteOn(midiEvent))
            endBeat = ((NoteOnEvent) midiEvent).AbsoluteTime / 480f;
        }
    }

    private void ResetTrack() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Input method.
    public void SongKey(InputAction.CallbackContext context) {        
        NoteType button = controlMapping[context.control.name];
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
    }

    private void ScoreManager(float add) {
        score += add*perfectNoteScore;
        scoreboard.text = ((int)score).ToString("D7");

        float diff = 1f/(chartNotes.Count*6f);
        
        if (add < 0.25) {
            drive -= diff * 4;
        }
        else if (add < 0.5f) {
            drive -= diff * 2;
        }
        else if (add < 0.75f) {
            drive += 0;
        }
        else if (add < 1f) {
            drive += diff;
        }
        else {
            drive += diff * 2;
        }

        drive = Mathf.Clamp01(drive);
        
        driveGauge.fillAmount = drive;
    }

    private void OffsetBarManager(float offset) {
        ob.MakeMark(offset);
    }

    private void ControlMapper() {
        controlMapping.Add("s", NoteType.L1);
        controlMapping.Add("d", NoteType.L2);
        controlMapping.Add("f", NoteType.L3);
        controlMapping.Add("k", NoteType.L4);
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