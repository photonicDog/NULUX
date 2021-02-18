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
	public CinemachineVirtualCamera currentCam;

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
			_currentScenario.scenarioNPCS.Add(new WalkaroundNPCState(scenarioNPC.name, scenarioNPC.state, scenarioNPC.nodes));
		}
		
		LoadNPCs();

		foreach (Room room in rooms.Values) {
			foreach (CinemachineVirtualCamera vc in room.RoomCameras.Values) {
				vc.gameObject.SetActive(false);
			}
		}
	}

	private void LoadNPCs() {
		List<ObjectConfig> npcs = FindObjectsOfType<ObjectConfig>().Where(a => a.IsStated).ToList();
		foreach (WalkaroundNPCState scenarioNPC in _currentScenario.scenarioNPCS) {
			ObjectConfig npc = npcs.Find(a => a.ID.Equals(scenarioNPC.name));
			if (npc == null) continue;
			npc.npcState.state = scenarioNPC.state;
			npc.npcState.nodes = scenarioNPC.nodes;
		}
	}

	public void ChangeNPCState(string id, int state) {
		FindObjectsOfType<ObjectConfig>()
			.Where(a => a.IsStated)
			.ToList()
			.Find(a => a.ID == id)
			.npcState.state = state;
	}

	public void ReadPotentialInteraction(ObjectConfig interactable)
	{
		currentInteractable = interactable;
	}

	public void SetCamera(CinemachineVirtualCamera vc) {
		if (currentCam) currentCam.gameObject.SetActive(false);
		vc.gameObject.SetActive(true);
		currentCam = vc;
	}

	public void ExecuteDialogueInteraction(InputAction.CallbackContext context)
	{
		if (context.started && currentInteractable != null)
		{
			sys.DeactivateInput();
			if (currentInteractable.IsDialogueTrigger)
			{
				DialogueRunner.StartDialogue(currentInteractable.npcState.GetDialogueByState());
			}
			else if (currentInteractable.IsSimpleDialogue)
			{
				DialogueRunner.StartDialogue(currentInteractable.dialogueKey);
			}
		}
	}

	public void UseDoor(GameObject mover, string room, string camera, Door door)
	{
		StartCoroutine(DoorAnimation(mover, room, camera, door));
	}

	private IEnumerator DoorAnimation(GameObject mover, string room, string camera, Door door)
	{
		sys.DeactivateInput();
		backgroundImage.color = Color.black - new Color(0f, 0f, 0f, 1f);
		while (backgroundImage.color.a < 1f)
		{
			backgroundImage.color += new Color(0f, 0f, 0f, 0.02f);
			yield return new WaitForEndOfFrame();
		}
		backgroundImage.color = Color.black;
		
		Vector3 position = door.destinationPosn.position;
		mover.transform.position = position;
		SetRoomContext(room);
		currentRoom.SwitchCamera(camera);
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
		SetCamera(currentRoom.RoomCameras.First().Value);
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
