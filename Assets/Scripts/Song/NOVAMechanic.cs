// NOVAMechanic
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Song;
using UnityEngine;
using UnityEngine.InputSystem;

public class NOVAMechanic : SerializedMonoBehaviour
{
	public delegate void ScoringSystem(ScoringJudgement score);
	public delegate void HandleCombo(int add);
	public delegate void RecordNoteData(HitData data);
	private HandleCombo Combo;
	private ScoringSystem Score;
	private RecordNoteData RecordData;
	
	private List<Note> notes;
	private Dictionary<Note, NOVANote> noteMap;
	private List<LineDataCommand> cmdList;
	private List<Note> currentlyActiveNotes;

	private List<NOVALine> novaLines;
	private List<NOVALine> activeLines;
	
	private Note[] _heldNotes;
	private List<HoldParticle> _holdParticles;
	
	[SerializeField] private GameObject noteObject;
	[SerializeField] private Transform noteCanvas;
	[SerializeField] private Dictionary<LineStyle, GameObject> novaPrefabs;
	[SerializeField] private WarningTelegraph warning;
	public FeedbackText fbtext;
	
	public float offset;

	private Dictionary<string, NoteType> _controlMapping;

	public List<ScoringJudgement> judgements;
	private float earlyCutoff;
	private float lateCutoff;
	[SerializeField] private ScoringJudgement missJudgement;
	
	[SerializeField] private float noteFadeInSpeed = 1f;	
	[SerializeField] private float lineFadeInSpeed = 1f;
	[SerializeField] private Vector2 minWindow;
	[SerializeField] private Vector2 maxWindow;

	public void Update()
	{
		if (Conductor.Instance.playing)
		{
			EliminateOffscreenNotes();
		}
	}

	private void FixedUpdate()
	{
		if (Conductor.Instance.playing)
		{
			UpdateLines();
			UpdateNotes();
		}
	}

	public void UpdateLines()
	{
		foreach (NOVALine novaLine in novaLines) {
			if (novaLine.currentCMD.startTime <= Conductor.Instance.GetSongTime() + novaLine.currentCMD.data.fadeLength && !novaLine.activated)
			{
				novaLine.FadeIn();
				activeLines.Add(novaLine);
			}
			novaLine.CalculateHitLineMovement(Conductor.Instance.GetSongTime());
		}
		
		List<NOVALine> slatedToRemove = new List<NOVALine>();
		foreach (NOVALine novaLine in activeLines) {
			
			if (novaLine.currentCMD.endTime < Conductor.Instance.GetSongTime()) {
				slatedToRemove.Add(novaLine);
			}
		}

		activeLines = activeLines.Except(slatedToRemove).ToList();
	}

	public void UpdateNotes()
	{
		foreach (Note item in notes)
		{
			if (noteMap.ContainsKey(item) && item.Start <= Conductor.Instance.GetSongTime() + noteFadeInSpeed && !noteMap[item].activated)
			{
				noteMap[item].PlaceNote(novaLines.Find(a => a.currentCMD.data.index == noteMap[item].note.Index));
				noteMap[item].FadeIn(noteFadeInSpeed);
				currentlyActiveNotes.Add(item);
			}
		}
	}

	public void ImportNotes(List<Note> notes, List<LineDataCommand> lineCommands)
	{
		this.notes = notes;
		
		cmdList = lineCommands;
		noteMap = new Dictionary<Note, NOVANote>();
		novaLines = new List<NOVALine>();
		activeLines = new List<NOVALine>();
		_holdParticles = new List<HoldParticle>();

		earlyCutoff = judgements.Select(a => a.leftMS).Min();
		lateCutoff = judgements.Select(a => a.rightMS).Max();

		SFXManager.Instance.LoadHitsounds(judgements);
		
		foreach (LineDataCommand ldc in cmdList) {
			GameObject go = Instantiate(novaPrefabs[ldc.data.Style]);
			NOVALine nl = go.GetComponent<NOVALine>();
			nl.currentCMD = ldc;
			novaLines.Add(nl);
			nl.SetActivated(false);
		}
		
		foreach (Note i in this.notes)
		{
			List<LineDataCommand> lineCommand = 
				cmdList
					.ToList()
					.FindAll((a) => i.Start >= a.startTime && i.Start <= a.endTime && i.Index == a.data.index);
			
			if (lineCommand.Count <= 0)
			{
				Debug.Log(string.Concat("Could not find a line for note ", i.NoteType, " at measure ", i.Start / 4f, 1));
			}
			else
			{
				NOVANote component = Instantiate(noteObject, new Vector3(9999f, 9999f, 0f), Quaternion.identity, noteCanvas).GetComponent<NOVANote>();
				noteMap.Add(i, component);
				component.Initialize(i, lineCommand);
			}
		}
		currentlyActiveNotes = new List<Note>();
	}

	public virtual void BuildMechanic(HandleCombo combo, ScoringSystem score, RecordNoteData recordData)
	{
		ControlMapper();
		Combo = (HandleCombo)Delegate.Combine(Combo, combo);
		Score = (ScoringSystem)Delegate.Combine(Score, score);
		RecordData = (RecordNoteData)Delegate.Combine(RecordData, recordData);
		_heldNotes = new Note[4];
		SFXManager.Instance.PlayAudio("song_hitsound");
		SFXManager.Instance.ResetAudio();
	}

	public void ToggleWarning()
	{
		warning.ShowWarning();
	}

	public void QueryInput(InputAction.CallbackContext context)
	{
		if (Conductor.Instance.playing)
		{
			float songTime = Conductor.Instance.GetSongTime();
			int translatedNote = (int)_controlMapping[context.action.name] % 4;
			if (context.canceled)
			{
				QueryEnded(translatedNote, songTime);
			}
			else if (context.started)
			{
				QueryStarted(translatedNote, songTime);
			}
		}
	}

	protected virtual void QueryStarted(int translatedNote, float currentPos)
	{
		if (notes.Count <= 0 || !notes.Exists((Note a) => a.NoteType == (NoteType)translatedNote))
		{
			return;
		}
		
		Note note = notes.First((Note a) => a.NoteType == (NoteType)translatedNote);
		if (note == null)
		{
			return;
		}
		
		if (note.Hit)
		{
			notes.Remove(note);
			QueryStarted(translatedNote, currentPos);
		}
		
		if (Mathf.Abs(PreciseHitTiming(note, 0f)) > lateCutoff)
		{
			//fbtext.ResetFeedback();
		} 
		else if ((int)note.NoteType % 4 == translatedNote) {
			ScoreLogic(note, PreciseHitTiming(note, 0f), releaseNote: false);
			notes.Remove(note);
			if (note.Duration > 0f)
			{
				_heldNotes[translatedNote] = note;
				StartCoroutine(QueryPerformed(translatedNote, currentPos));
			}
		}
	}

	private IEnumerator QueryPerformed(int translatedNote, float currentPos)
	{
		while (true) {
			foreach (Note note in _heldNotes) {
				if (note == null) continue;
				NOVANote nnote = noteMap[note];
				nnote.AnimateHold(Conductor.Instance.GetSongTime());
			}
			
			yield return new WaitForEndOfFrame();
		}
	}

	protected virtual void QueryEnded(int translatedNote, float currentPos)
	{
		Note note = _heldNotes[translatedNote];
		if (note != null)
		{
			StopCoroutine(QueryPerformed(translatedNote, currentPos));
			ScoreLogic(note, PreciseHitTiming(note, note.Duration), releaseNote: true);
			_heldNotes[translatedNote] = null;
		}
	}

	private float PreciseHitTiming(Note note, float holdOffset)
	{
		float hitTime = Conductor.Instance.GetSongTimeMS();
		float noteTime = 1f / (Conductor.Instance.bpm / 60f) * (note.Start + holdOffset) * 1000f;
		return (hitTime - noteTime);
	}

	private void ScoreLogic(Note note, float hitTiming, bool releaseNote, float lenience = 1f)
	{
		Transform textPosition = noteMap[note].head.transform;
		ScoringJudgement judge = judgements.Find(a => a.leftMS < hitTiming && a.rightMS >= hitTiming);

		if (judge != null) {
			fbtext.DisplayFeedback(judge.feedbackText, textPosition);
			Score(judge);
			Combo(judge.combo);
			PushInputToPart(noteMap[note], judge.countsAsHit, releaseNote);
			RecordData(new HitData(note.Start + (releaseNote?note.Duration:0), hitTiming, judge.heur));
			SFXManager.Instance.PlayHitsound(judge.leftMS);
		}
		else {
			fbtext.DisplayFeedback(missJudgement.feedbackText, textPosition);
			//Debug.Log(string.Concat("Did not hit note ", note.NoteType, " at ", note.Start, " with offset of ", hitTiming));
			Score(missJudgement);
			Combo(missJudgement.combo);
			PushInputToPart(noteMap[note], hit: false, releaseNote);
		}
		
		note.Hit = true;

		currentlyActiveNotes.Remove(note);
		EliminateOffscreenNotes();
	}

	public void EliminateOffscreenNotes()
	{
		if (currentlyActiveNotes.Count == 0)
		{
			return;
		}
		List<Note> list = new List<Note>();
		foreach (Note currentlyActiveNote in currentlyActiveNotes)
		{
			if (Conductor.Instance.playing && Conductor.Instance.GetSongTime() - currentlyActiveNote.Start >= 0.16f)
			{
				if (!currentlyActiveNote.Hit)
				{
					fbtext.DisplayFeedback(missJudgement.feedbackText, noteMap[currentlyActiveNote].transform);
					Combo(0);
					Score(missJudgement);
					currentlyActiveNote.Hit = true;
				}
				PushInputToPart(noteMap[currentlyActiveNote], hit: false, release: currentlyActiveNote.Duration > 0);
				//Debug.Log(string.Concat("Missed note ", currentlyActiveNote.NoteType, " at ", currentlyActiveNote.Start));
				list.Add(currentlyActiveNote);
			}
		}
		currentlyActiveNotes = currentlyActiveNotes.Except(list).ToList();
	}

	private void PushInputToPart(NOVANote note, bool hit, bool release)
	{
		if (!(note.note.Duration > 0f))
		{
			// Tap note destruction logic
			note.Kill(hit);
			Destroy(note.gameObject);
			noteMap.Remove(note.note);
			notes.Remove(note.note);
		} else {
			if (release) {
				// Release note destruction logic
				note.EndAnimateHold();
				Destroy(note.gameObject);
				noteMap.Remove(note.note);
				notes.Remove(note.note);

			}
			else {
				note.StartAnimateHold();
			}
		}
	}

	private void ControlMapper()
	{
		_controlMapping = new Dictionary<string, NoteType>
		{
			{
				"L1",
				NoteType.L1
			},
			{
				"L2",
				NoteType.L2
			},
			{
				"L3",
				NoteType.L3
			},
			{
				"L4",
				NoteType.L4
			}
		};
	}
}

public enum ScoringHeuristic {
	STELLAR,
	PERFECT,
	GREAT,
	GOOD,
	MISS
}

[Serializable]
public class ScoringJudgement {
	[HorizontalGroup("Data")]
	[BoxGroup("Data/Judgement")] [LabelWidth(100)] public ScoringHeuristic heur;
	[BoxGroup("Data/Judgement")] [LabelWidth(100)]public int leftMS;
	[BoxGroup("Data/Judgement")] [LabelWidth(100)]public int rightMS;
	
	[BoxGroup("Data/Score")] [LabelWidth(100)] public float scoreMultiplier;
	[BoxGroup("Data/Score")][LabelWidth(100)] public float clearMultiplier;
	[BoxGroup("Data/Score")][LabelWidth(100)]public int combo;
	[BoxGroup("Data/Score")][LabelWidth(100)]public bool countsAsHit;
	
	[BoxGroup("Feedback")] public GameObject feedbackText;
	[BoxGroup("Feedback")] public AudioClip feedbackAudio;

}
