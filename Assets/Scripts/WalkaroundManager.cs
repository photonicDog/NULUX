// WalkaroundManager
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WalkAround.Objects.Implementations;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class WalkaroundManager : SerializedMonoBehaviour
{
	public static WalkaroundManager Instance;

	public ImageAssetKey bgImages;
	public ImageAssetKey fgImages;

	public DialogueRunner DialogueRunner;
	public DialogueTalkspriter Talkspriter;
	public PlayerInput sys;
	public WalkaroundNPCScenarioState currentScenario;

	[HideInInspector]
	public WalkaroundNPCScenarioState _currentScenario;

	[SerializeField]
	private ObjectConfig currentInteractable;

	public Image backgroundImage;

	public Dictionary<string, Room> rooms;
	[HideInInspector] public Room currentRoom;

	private CinemachineBrain brain;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		brain = Camera.main.GetComponent<CinemachineBrain>();
		_currentScenario = ScriptableObject.CreateInstance<WalkaroundNPCScenarioState>();
		_currentScenario.scenarioNPCS = new List<WalkaroundNPCState>();
		foreach (WalkaroundNPCState scenarioNPC in currentScenario.scenarioNPCS)
		{
			_currentScenario.scenarioNPCS.Add(new WalkaroundNPCState(scenarioNPC.name, scenarioNPC.state, scenarioNPC.talksprites, scenarioNPC.npcBody, scenarioNPC.nodes));
		}
		Talkspriter.ImportAllTalksprites((from a in _currentScenario.scenarioNPCS
			select a.talksprites into a
			where a != null
			select a).ToList());

		foreach (Room room in rooms.Values) {
			foreach (CinemachineVirtualCamera vc in room.RoomCameras.Values) {
				vc.gameObject.SetActive(false);
			}
		}
	}

	public void ReadPotentialInteraction(ObjectConfig interactable)
	{
		currentInteractable = interactable;
	}

	public void SetCamera(CinemachineVirtualCamera vc) {
		brain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
		vc.VirtualCameraGameObject.SetActive(true);
		
	}

	public void ExecuteDialogueInteraction(InputAction.CallbackContext context)
	{
		if (context.started && currentInteractable != null)
		{
			sys.DeactivateInput();
			if (currentInteractable.IsDialogueTrigger)
			{
				DialogueRunner.StartDialogue(_currentScenario.GetCurrentNode(currentInteractable.Key));
			}
			else if (currentInteractable.IsLookable)
			{
				DialogueRunner.StartDialogue(currentInteractable.Key);
			}
		}
	}

	public void UseDoor(GameObject mover, Door door)
	{
		StartCoroutine(DoorAnimation(mover, door));
	}

	private IEnumerator DoorAnimation(GameObject mover, Door door)
	{
		sys.DeactivateInput();
		backgroundImage.color = Color.black - new Color(0f, 0f, 0f, 1f);
		while (backgroundImage.color.a < 1f)
		{
			backgroundImage.color += new Color(0f, 0f, 0f, 0.02f);
			yield return new WaitForEndOfFrame();
		}
		backgroundImage.color = Color.black;
		Vector3 position = door.destinationPosn;
		mover.transform.position = position;
		yield return new WaitForSeconds(0.2f);
		while (backgroundImage.color.a > 0f)
		{
			backgroundImage.color -= new Color(0f, 0f, 0f, 0.02f);
			yield return new WaitForEndOfFrame();
		}
		backgroundImage.color = Color.black - new Color(0f, 0f, 0f, 1f);
		sys.ActivateInput();
	}

	public void SetRoomContext(string key) {
		currentRoom = rooms[key];
	}

	public void ReactivatePlayerControls()
	{
		sys.ActivateInput();
	}

	public void ReturnToMenu()
	{
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
		}
	}
}
