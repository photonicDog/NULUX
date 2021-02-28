// NOVAMechanic
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Song;
using UnityEngine;
using UnityEngine.InputSystem;

public class NOVAMechanic : SerializedMonoBehaviour
{
	public delegate void ScoringSystem(ScoringHeuristic score);

	public delegate void HandleCombo(int add);

	public delegate void AddToOffsetBar(float offset);

	protected List<Note> notes;

	protected Dictionary<Note, NOVANote> noteMap;

	protected List<LineDataCommand> cmdList;

	protected List<Note> currentlyActiveNotes;

	[SerializeField]
	private GameObject noteObject;

	[SerializeField]
	private Transform noteCanvas;

	[SerializeField]
	private List<NOVALine> novaLines;
	
	[SerializeField]
	private Dictionary<LineStyle, GameObject> novaPrefabs;


	public float offset;

	protected Dictionary<string, NoteType> _controlMapping;

	public float scrollSpeed;

	[SerializeField]
	private WarningTelegraph warning;

	protected HandleCombo Combo;

	protected ScoringSystem Score;

	protected AddToOffsetBar OffsetBar;

	public FeedbackText fbtext;

	private Note[] _heldNotes;

	[SerializeField]
	private float perfectWindow = 20f;

	[SerializeField]
	private float greatWindow = 40f;

	[SerializeField]
	private float goodWindow = 90f;

	[SerializeField]
	private float noteFadeInSpeed = 1f;

	[SerializeField]
	private float lineFadeInSpeed = 1f;

	[SerializeField]
	private Vector2 minWindow;

	[SerializeField]
	private Vector2 maxWindow;

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
			}
			
			novaLine.CalculateHitLineMovement(Conductor.Instance.GetSongTime());
		}
		
		
	}

	public void UpdateNotes()
	{
		foreach (Note item in notes)
		{
			if (item.Start <= Conductor.Instance.GetSongTime() + noteFadeInSpeed && !noteMap[item].activated)
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

	public virtual void BuildMechanic(HandleCombo combo, ScoringSystem score, AddToOffsetBar offsetBar)
	{
		ControlMapper();
		Combo = (HandleCombo)Delegate.Combine(Combo, combo);
		Score = (ScoringSystem)Delegate.Combine(Score, score);
		OffsetBar = (AddToOffsetBar)Delegate.Combine(OffsetBar, offsetBar);
		_heldNotes = new Note[4];
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
			else if (context.performed)
			{
				QueryPerformed(translatedNote, songTime);
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
			fbtext.ResetFeedback();
			QueryStarted(translatedNote, currentPos);
		}
		if (Mathf.Abs(PreciseHitTiming(note, 0f)) > goodWindow)
		{
			fbtext.ResetFeedback();
		}
		else if ((int)note.NoteType % 4 == translatedNote)
		{
			if (note.Duration > 0f)
			{
				_heldNotes[translatedNote] = note;
			}
			ScoreLogic(note, PreciseHitTiming(note, 0f), releaseNote: false);
			notes.Remove(note);
		}
	}

	private void QueryPerformed(int translatedNote, float currentPos)
	{
	}

	protected virtual void QueryEnded(int translatedNote, float currentPos)
	{
		Note note = _heldNotes[translatedNote];
		if (note != null)
		{
			ScoreLogic(note, PreciseHitTiming(note, note.Duration), releaseNote: true);
			_heldNotes[translatedNote] = null;
		}
	}

	private float PreciseHitTiming(Note note, float holdOffset)
	{
		float num = Conductor.Instance.GetSongTimeMS();
		float num2 = 1f / (Conductor.Instance.bpm / 60f) * (note.Start + holdOffset) * 1000f;
		return -1f * (num - num2);
	}

	private void ScoreLogic(Note note, float hitTiming, bool releaseNote)
	{
		ScoreLogic(note, hitTiming, releaseNote, 1f);
	}

	private void ScoreLogic(Note note, float hitTiming, bool releaseNote, float lenience)
	{
		OffsetBar(hitTiming);
		if (Mathf.Abs(hitTiming) < perfectWindow * lenience)
		{
			fbtext.DisplayFeedback(0, noteMap[note].transform);
			Score(ScoringHeuristic.PERFECT);
			Combo(1);
			PushInputToPart(noteMap[note], hit: true, releaseNote);
			//Debug.Log(string.Concat("Perfect note ", note.NoteType, " at ", note.Start, " with offset of ", hitTiming));
		}
		else if (Mathf.Abs(hitTiming) < greatWindow * lenience)
		{
			fbtext.DisplayFeedback(1, noteMap[note].transform);
			Score(ScoringHeuristic.GREAT);
			Combo(1);
			PushInputToPart(noteMap[note], hit: true, releaseNote);
			//Debug.Log(string.Concat("Great note ", note.NoteType, " at ", note.Start, " with offset of ", hitTiming));
		}
		else if (Mathf.Abs(hitTiming) <= goodWindow * lenience || (releaseNote && hitTiming > 0f))
		{
			fbtext.DisplayFeedback(2, noteMap[note].transform);
			Score(ScoringHeuristic.GOOD);
			Combo(0);
			PushInputToPart(noteMap[note], hit: true, releaseNote);
			//Debug.Log(string.Concat("Good note ", note.NoteType, " at ", note.Start, " with offset of ", hitTiming));
		}
		else
		{
			fbtext.DisplayFeedback(3, noteMap[note].transform);
			//Debug.Log(string.Concat("Did not hit note ", note.NoteType, " at ", note.Start, " with offset of ", hitTiming));
			PushInputToPart(noteMap[note], hit: false, releaseNote);
			Score(ScoringHeuristic.MISS);
			Combo(0);
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
					fbtext.DisplayFeedback(3, noteMap[currentlyActiveNote].transform);
					Combo(0);
					Score(0f);
					currentlyActiveNote.Hit = true;
				}
				PushInputToPart(noteMap[currentlyActiveNote], hit: false, release: false);
				Debug.Log(string.Concat("Missed note ", currentlyActiveNote.NoteType, " at ", currentlyActiveNote.Start));
				list.Add(currentlyActiveNote);
			}
		}
		currentlyActiveNotes = currentlyActiveNotes.Except(list).ToList();
	}

	private void PushInputToPart(NOVANote note, bool hit, bool release)
	{
		if (!hit)
		{
			note.Kill(hit);
			noteMap.Remove(note.note);
			notes.Remove(note.note);
		}
		if (release || !(note.note.Duration > 0f))
		{
			note.Kill(hit);
			noteMap.Remove(note.note);
			notes.Remove(note.note);
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
	PERFECT,
	GREAT,
	GOOD,
	MISS
}
