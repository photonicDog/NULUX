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
	public CinemachineBrain spriteCamBrain;

	[HideInInspector]
	public WalkaroundNPCScenarioState _currentScenario;

	[SerializeField]
	private ObjectConfig currentInteractable;

	public Image backgroundImage;

	public Dictionary<string, Room> rooms;
	[HideInInspector] public Room currentRoom;
	

	private CinemachineBrain brain;
	public WalkaroundCamera currentCam;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start() {
		brain = Camera.main.GetComponent<CinemachineBrain>();
		_currentScenario = ScriptableObject.CreateInstance<WalkaroundNPCScenarioState>();
		_currentScenario.scenarioNPCS = new List<WalkaroundNPCState>();
		foreach (WalkaroundNPCState scenarioNPC in currentScenario.scenarioNPCS)
		{
			_currentScenario.scenarioNPCS.Add(new WalkaroundNPCState(scenarioNPC.name, scenarioNPC.state, scenarioNPC.nodes));
		}
		
		LoadNPCs();

		foreach (Room room in rooms.Values) {
			foreach (WalkaroundCamera vc in room.RoomCameras) {
				vc.DeactivateCamera();
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

	public void SetNPCState(string id, int state) {
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

	public void SetCamera(WalkaroundCamera vc) {
		if (currentCam) currentCam.DeactivateCamera();
		vc.ActivateCamera();
		currentCam = vc;
	}

	public void ExecuteDialogueInteraction(InputAction.CallbackContext context)
	{
		if (context.started && currentInteractable != null)
		{
			sys.DeactivateInput();
			sys.GetComponent<ObjectController>().InspectBubble(false);
			if (currentInteractable.IsDialogueTrigger)
			{
				DialogueRunner.StartDialogue(currentInteractable.npcState.GetDialogueByState());
			}
			else if (currentInteractable.IsSimpleDialogue)
			{
				DialogueRunner.StartDialogue(currentInteractable.dialogueKey);
			}
			else {
				sys.ActivateInput();
			}
		}
	}

	public void UseDoor(GameObject mover, Room room, WalkaroundCamera camera, Door door)
	{
		StartCoroutine(DoorAnimation(mover, room, camera, door));
	}

	private IEnumerator DoorAnimation(GameObject mover, Room room, WalkaroundCamera camera, Door door)
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
		SetCamera(camera);
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
		SetRoomContext(rooms[key]);
		SetCamera(currentRoom.RoomCameras.First());
	}

	public void SetRoomContext(Room room) {
		currentRoom = room;
		SetCamera(currentRoom.RoomCameras.First());
	}

	public void ReactivatePlayerControls()
	{
		sys.ActivateInput();
	}

	public void SwitchSprite(string name, string index) {
		ObjectConfig sprite = currentRoom.transform.Find("InteractiveFurniture").Find(name).GetComponent<ObjectConfig>();
		if (sprite.IsSwitchableSprite) {
			sprite.switcher.SwitchSprite(index);
		}
	}

	public void ReturnToMenu()
	{
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
		}
	}
}
