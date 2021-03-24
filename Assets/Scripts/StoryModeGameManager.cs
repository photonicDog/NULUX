// StoryModeGameManager
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Object = System.Object;

public class StoryModeGameManager : MonoBehaviour
{
	public delegate void StateUpdate();

	public static StoryModeGameManager Instance;

	public Gamestate gamestate;

	public Gamestate _gamestate;

	public List<DialogueCondition> flnodes;

	[SerializeField]
	private List<DialogueCondition> _flnodes;

	public StateUpdate stateUpdate;

	private List<string> completedNodes;

	public DialogueRunner dl;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("Multiple StoryManagers in the scene!");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		_gamestate = UnityEngine.Object.Instantiate(gamestate);
		SceneManager.sceneLoaded += GrabDialogueManager;
		foreach (DialogueCondition flnode in flnodes)
		{
			_flnodes.Add(UnityEngine.Object.Instantiate(flnode));
		}
	}

	private void GrabDialogueManager(Scene scene, LoadSceneMode mode)
	{
		dl = GameObject.Find("Dialogue Runner").GetComponent<DialogueRunner>();
	}

	private void Update()
	{
		UpdateSkits();
	}

	private void UpdateSkits()
	{
		foreach (DialogueCondition flnode in _flnodes)
		{
			Gamestate g = _gamestate;
			if (!flnode.played
			    && flnode.location == SceneManager.GetActiveScene().buildIndex
			    && flnode.flags.Where(a => g.flags.Exists(b => b.id == a.id))
				    .All(a => g.flags.Find(c => a.id == c.id).flag == a.flag))
			{
				dl.StartDialogue(flnode.node);
				flnode.played = true;
			}
		}
	}

	public void SetGamestateFlag(string flag, bool value)
	{
		_gamestate.SetFlag(flag, value);
		
		try
		{
			stateUpdate();
		}
		catch (NullReferenceException)
		{
		}
	}

	public void BuildSong(string trackname)
	{
		StartCoroutine(OnBuildSong(trackname));
		LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Song"));
	}

	private IEnumerator OnBuildSong(string trackname)
	{
		BGMManager.Instance.KillAudio();
		SFXManager.Instance.KillAudio();
		while (!GameObject.Find("SongManager"))
		{
			yield return new WaitForEndOfFrame();
		}
		GameObject.Find("SongManager").GetComponent<SongManager>().SetCurrentTrack(_gamestate.tracks[trackname]);
	}
}
