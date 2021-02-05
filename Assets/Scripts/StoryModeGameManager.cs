// StoryModeGameManager
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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

	public void RegisterNodeAsCompleted()
	{
	}

	private void UpdateSkits()
	{
		foreach (DialogueCondition flnode in _flnodes)
		{
			Gamestate g = _gamestate;
			if (!flnode.played 
			    && flnode.location == SceneManager.GetActiveScene().buildIndex 
			    && flnode.integerFlags
				    .Where((KeyValuePair<string, int> a) => g.integerFlags
					    .ContainsKey(a.Key)).All((KeyValuePair<string, int> a) => a.Value == g.integerFlags[a.Key]) 
			    && flnode.booleanFlags
				    .Where((KeyValuePair<string, bool> a) => g.booleanFlags
					    .ContainsKey(a.Key)).All((KeyValuePair<string, bool> a) => a.Value == g.booleanFlags[a.Key]))
			{
				dl.StartDialogue(flnode.node);
				flnode.played = true;
			}
		}
	}

	public void SetGamestateFlag(string flag, bool value)
	{
		_gamestate.booleanFlags[flag] = value;
		try
		{
			stateUpdate();
		}
		catch (NullReferenceException)
		{
		}
	}

	public void SetGamestateFlag(string flag, int value)
	{
		_gamestate.integerFlags[flag] = value;
		try
		{
			stateUpdate();
		}
		catch (NullReferenceException)
		{
		}
	}

	public void SetGamestateFlag(Dictionary<string, bool> over)
	{
		foreach (KeyValuePair<string, bool> item in over)
		{
			_gamestate.booleanFlags[item.Key] = item.Value;
		}
		try
		{
			stateUpdate();
		}
		catch (NullReferenceException)
		{
		}
	}

	public void SetGamestateFlag(Dictionary<string, int> over)
	{
		foreach (KeyValuePair<string, int> item in over)
		{
			_gamestate.integerFlags[item.Key] = item.Value;
		}
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
