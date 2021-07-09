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

public class SongManagerDeprecated : SerializedMonoBehaviour {
    // TODO: Organize this PLEASE
    public static SongManagerDeprecated Instance;
    
    public Track currentTrack;
    private List<LineDataCommand> lineDataCommands;
    private Queue<TimingEvent> timingQueue;

    private Note[] heldNotes;
    private Queue<ChartDialogueCommand> dialogueCommandQueue;

    [SerializeField] private GameObject introAnimation = default;
    [SerializeField] private AudioSource introAnimationSound = default;
    
    [SerializeField] private GameObject mechanicObject = default;
    private NOVAMechanicDeprecated _mechanicDeprecated;
    [SerializeField] private AudioSource songAudio;

    public float score = 0;
    public TextMeshProUGUI scoreboard;
    [SerializeField] private SongUIManagerDeprecated ui;
    public DialogueTalkspriter Talkspriter;
    private ResultsPanelDeprecated resultsPanel;
    [HideInInspector] public Dictionary<ScoringHeuristic, int> noteResults;

    [SerializeReference] private DialogueRunner dialogueRunner = default;
    [SerializeReference] private Button dialogueAdvanceButton = default;

    private float perfectNoteScore;

    public bool isStoryMode;

    public Image driveGauge;
    private float drive = 0f;
    private float pacemaker = 0f;

    private List<Note> chartNotes;
    private int origNoteCt;

    [SerializeField] private OffsetBar ob = default;
    [HideInInspector] public List<HitDataDeprecated> recordedData;

    public int combo;
    private float endTime;

    public Image endFade;

    void Awake() {
        Instance = this;
        StartCoroutine(WaitForSongStart());
    }

    private void OnDestroy() {
        Instance = null;
    }

    public void SetCurrentTrack(Track t) {
        currentTrack = t;
        isStoryMode = t.isStoryModeTrack;
    }
    
    // -- SETUP --

    IEnumerator WaitForSongStart() {
        while (currentTrack == null) {
            yield return new WaitForFixedUpdate();
        }
        InputSystem.pollingFrequency = 4000f;
        
        InitializeSong();
    }

    void InitializeSong() {
        AssembleMechanic();
        AssembleChart();
        AssembleData();
        AssembleUI();
        
        StartCoroutine(StartSequence());
    }
    void AssembleMechanic() {
        _mechanicDeprecated = mechanicObject.GetComponent<NOVAMechanicDeprecated>();
        _mechanicDeprecated.BuildMechanic(ComboManager, ScoreManager, RecordDataManager);
    }
    void AssembleChart() {
        chartNotes = new List<Note>();
        lineDataCommands = new List<LineDataCommand>();
        
        heldNotes = new Note[4];
        for (int i = 0; i < 4; i++) {
            heldNotes[i] = null;
        }
        
        ReadMIDI(Application.streamingAssetsPath + "/" + currentTrack.PathToChart +".mid", currentTrack);
        songAudio.clip = currentTrack.Audio;
        origNoteCt = chartNotes.Count;
    }
    void AssembleData() {
        resultsPanel = GetComponent<ResultsPanelDeprecated>();
        recordedData = new List<HitDataDeprecated>();
        noteResults = new Dictionary<ScoringHeuristic, int>() {
            {ScoringHeuristic.STELLAR, 0},
            {ScoringHeuristic.PERFECT, 0},
            {ScoringHeuristic.GREAT, 0},
            {ScoringHeuristic.GOOD, 0},
            {ScoringHeuristic.MISS, 0}
        };
        
        perfectNoteScore = 1000000f / ((chartNotes.Count));
    }
    void AssembleUI() {
        if (currentTrack.yp) dialogueRunner.Add(currentTrack.yp);
        driveGauge.fillAmount = drive;
        
        ui.UpdateSongAttributes(currentTrack.trackName);
    }

    IEnumerator StartSequence() {
        introAnimation.SetActive(true);
        introAnimationSound.Play();
        yield return new WaitForSeconds(7f);
        introAnimation.SetActive(false);
        Conductor.Instance.StartMusic();
    }
    
   // -- GAMEPLAY LOOP -- 
   
   void Update() {
        if (Conductor.Instance.isPlaying && dialogueCommandQueue.Count > 0) {
            DialogueCheck();
        }
    }
   
   #region DIALOGUE_HANDLING
   void DialogueCheck() {
       if (CheckValueAgainstTiming(dialogueCommandQueue.Peek().timing)) {
           ChartDialogueCommand dcm = dialogueCommandQueue.Dequeue();
//           Debug.Log("Running command " + dcm.command[0]);
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
   private void RunDialogueNode(string node) {
        dialogueRunner.StartDialogue(node);
    }

    private void AdvanceDialogueNode() {
        dialogueAdvanceButton.onClick.Invoke();
    }
    
    #endregion

    private void LateUpdate() {
        if (Conductor.Instance.isPlaying) {
            CheckEnd();
        }
    }

    void CheckEnd() {
        if (Conductor.Instance.songTime >= endTime) {
            songAudio.Stop();
            StartCoroutine(EndSong());
        }
    }

    IEnumerator EndSong() {
        yield return StartCoroutine(FadeOut(120f));
        
        mechanicObject.SetActive(false);
        resultsPanel.endBeat = endTime;
        resultsPanel.SetScore((int)score);
    }

    IEnumerator FadeOut(float timeInFrames) {
        while (endFade.color.a < 0.99) {
            endFade.color += new Color(0,0,0,1/timeInFrames);
            yield return new WaitForEndOfFrame();
        }
    }
    
    #region MIDI

    void MIDITempoTrack(MidiFile mf) {
        foreach (var midiEvent in mf.Events[0]) {
            if (midiEvent is TempoEvent) {
                timingQueue.Enqueue(new TimingEvent(midiEvent.AbsoluteTime, (float)(midiEvent as TempoEvent).Tempo));
            } else if (midiEvent is TextEvent) {
                if ((midiEvent as TextEvent).MetaEventType == MetaEventType.TextEvent) 
                    Conductor.Instance.startOffset = (float.Parse((midiEvent as TextEvent).Text));
            }
        }
    }

    void MIDINoteTrack(MidiFile mf) {
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
    }

    void MIDILineTrack(MidiFile mf) {
        // TODO: FIX THIS
        foreach (var midiEvent in mf.Events[2]) {
            if (MidiEvent.IsNoteOn(midiEvent)) {
                NoteOnEvent nev = midiEvent as NoteOnEvent;
                TrackLines asscTrack = currentTrack.Lines;
                lineDataCommands.Add(new LineDataCommand(
                    asscTrack.trackLines[nev.NoteNumber], 
                    nev.AbsoluteTime/480f, 
                    nev.AbsoluteTime/480f + (nev.NoteLength-1)/480f)
                );
            }
        }
    }

    void MIDIDialogueTrack(MidiFile mf) {
        if (currentTrack.yp != null) {
            foreach (var midiEvent in mf.Events[3]) {
                if (midiEvent is TextEvent && ((TextEvent) midiEvent).MetaEventType == MetaEventType.Marker) {
                    TextEvent tev = midiEvent as TextEvent;
                    string[] cmds = tev.Text.Split(' ');
                    dialogueCommandQueue.Enqueue(new ChartDialogueCommand(cmds, (float) midiEvent.AbsoluteTime / 480));
                }
            }
        }
    }

    void MIDIEndTrack(MidiFile mf) {
        foreach (var midiEvent in mf.Events[4]) {
            if (MidiEvent.IsNoteOn(midiEvent)) {
                endTime = ((NoteOnEvent) midiEvent).AbsoluteTime / 480f;
                break;
            }
        }
    }

    #endregion
    
    // Read midi from path.
    void ReadMIDI(string midiPath, Track track) {
        MidiFile mf = new MidiFile(midiPath, false);
        dialogueCommandQueue = new Queue<ChartDialogueCommand>();
        
        MIDITempoTrack(mf);
        MIDINoteTrack(mf);
        MIDILineTrack(mf);
        MIDIDialogueTrack(mf);
        MIDIEndTrack(mf);

        chartNotes.Select(a => new Note(a.Start, a.Index, a.Position, a.Duration));
        _mechanicDeprecated.ImportNotes(chartNotes, lineDataCommands);
        
        //End track

    }

    private void ResetTrack() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void RecordDataManager(HitDataDeprecated data) {
        recordedData.Add(data);
        ob.MakeMark(data.offset, data.heur);
    }
    
    public void ReturnToMenu() {
        if (SceneManager.GetActiveScene().buildIndex == 4)
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
    }
    
    // -- UTILITY --

    bool CheckValueAgainstTiming(double timing) {
        return timing < Conductor.Instance.songTime;
    }

    double MidiToSeconds(float resolution, float absoluteTime, float bpm) {
        return (absoluteTime / resolution) / bpm;
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