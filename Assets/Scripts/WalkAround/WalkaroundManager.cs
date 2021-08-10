// WalkaroundManager

using System.Collections.Generic;
using System.Linq;
using Dialogue_System;
using ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yarn.Unity;

namespace WalkAround {
	public class WalkaroundManager : SerializedMonoBehaviour
	{
		public static WalkaroundManager Instance;

		[Header("Cutscene Image Panels")]
		public ImageAssetKey bgImages;
		public ImageAssetKey fgImages;
		[FormerlySerializedAs("backgroundImage")] public Image fadeImage;

		[Header("System Managers")]
		public DialogueRunner DialogueRunner;
		public DialogueTalkspriter Talkspriter;
		public PlayerInput sys;
		[HideInInspector] public RoomManager RoomManager;
		[HideInInspector] public WalkaroundCameraManager CameraManager;

		[Header("Scenario")]
		public WalkaroundNPCScenarioState currentScenario;
		[HideInInspector] public WalkaroundNPCScenarioState _currentScenario;
		[SerializeField] private ObjectConfig currentInteractable;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}

			RoomManager = GetComponent<RoomManager>();
			CameraManager = GetComponent<WalkaroundCameraManager>();
		}

		void Start() {
			_currentScenario = UnityEngine.ScriptableObject.CreateInstance<WalkaroundNPCScenarioState>();
			_currentScenario.scenarioNPCS = new List<WalkaroundNPCState>();
			foreach (WalkaroundNPCState scenarioNPC in currentScenario.scenarioNPCS)
			{
				_currentScenario.scenarioNPCS.Add(new WalkaroundNPCState(scenarioNPC.name, scenarioNPC.state, scenarioNPC.nodes));
			}
		
			LoadNPCs();
			CameraManager.Initialize();
			RoomManager.Initialize(fadeImage, sys, CameraManager);
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

		public void ReactivatePlayerControls()
		{
			sys.ActivateInput();
		}

		public void SwitchSprite(string name, string index) {
			ObjectConfig sprite = RoomManager.currentRoom.SwitchableObject[name];
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
}
