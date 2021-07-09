using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Debug = UnityEngine.Debug;

public class DialogueCommandHandler : MonoBehaviour {
    public CutsceneBG CutsceneBg;
    public CutsceneBG CutsceneBgBuffer;
    public CutsceneFG CutsceneFg;

    public DialogueRunner runner;
    public GameObject DialogueContainer;
    private WalkaroundNPCState currentNPCState;

    void Awake() {
        runner.AddCommandHandler(
            "setnpcstate",
            SetNPCState);
        runner.AddCommandHandler(
            "setbg",
            SetBG);
        runner.AddCommandHandler(
            "clearbg",
            ClearBG);
        runner.AddCommandHandler(
            "setfg",
            SetFG);
        runner.AddCommandHandler(
            "clearfg",
            ClearFG);
        runner.AddCommandHandler(
            "spawnchar",
            SpawnChar);
        runner.AddCommandHandler(
            "despawnchar",
            DespawnChar);
        runner.AddCommandHandler(
            "walkchar",
            WalkChar);
        runner.AddCommandHandler(
            "walkcharblocking",
            WalkCharBlocking);
        runner.AddCommandHandler(
            "setexpression",
            SetCharExpression);
        runner.AddCommandHandler(
            "setflag",
            SetFlag);
        runner.AddCommandHandler(
            "switchscene",
            SwitchScene);
        runner.AddCommandHandler(
            "setbgm",
            SwitchBGM);
        runner.AddCommandHandler(
            "killbgm",
            CutBGM);
        runner.AddCommandHandler(
            "playsfx",
            PlaySFX);
        runner.AddCommandHandler(
            "playsfxblocking",
            PlaySFXBlocking);
        runner.AddCommandHandler(
            "moveplayer",
            SpawnPlayer);
        runner.AddCommandHandler(
            "customwait",
            CustomWait);
        runner.AddCommandHandler(
            "setobjproperties",
            SetObjectProperties);
        runner.AddCommandHandler(
            "loadtrack",
            BuildSong);
        runner.AddCommandHandler(
            "setroom",
            SetRoomContext);
        runner.AddCommandHandler(
            "setlight",
            SetLight);
        runner.AddCommandHandler(
            "switchcam",
            SwitchCam);
        runner.AddCommandHandler(
            "switchsprite",
            SwitchSprite);
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void SetNPCState(string[] parameters) {
        WalkaroundManager.Instance.SetNPCState(parameters[0], int.Parse(parameters[1]));
    }

    void SetBG(string[] parameters, System.Action onComplete) {
        if (parameters[0].Equals("color")) {
            Color c;
            ColorUtility.TryParseHtmlString(parameters[1], out c);

            if (parameters[2] == "0") {
                CutsceneBg.SetBGInstant(c);
                onComplete();
            }
            else
                CutsceneBg.SetBG(c, int.Parse(parameters[2]), onComplete, DialogueContainer);
        }

        if (parameters[0].Equals("image")) {
            if (parameters[2] == "0") {
                CutsceneBg.SetBGInstant(WalkaroundManager.Instance.bgImages.LookupAsset(parameters[1]));
                onComplete();
            }
            else
                CutsceneBgBuffer.SetBG(WalkaroundManager.Instance.bgImages.LookupAsset(parameters[1]),
                    int.Parse(parameters[2]), CutsceneBg, onComplete, DialogueContainer);
        }
    }

    void ClearBG(string[] parameters) {
        CutsceneBg.ClearBG();
    }

    void SetFG(string[] parameters) {
        float x = float.Parse(parameters[1]);
        float y = float.Parse(parameters[2]);
        float w = 0;
        float h = 0;
        if (WalkaroundManager.Instance != null) {
            CutsceneFg.SetFG(WalkaroundManager.Instance.fgImages.LookupAsset(parameters[0]), x, y, w, h);
        }
        else {
            CutsceneFg.SetFG(SongManagerDeprecated.Instance.currentTrack.imgKey.LookupAsset(parameters[0]), x, y, w, h);
        }

    }

    void ClearFG(string[] parameters) {
        CutsceneFg.ClearFG();
    }

    void SetRoomContext(string[] parameters) {
        WalkaroundManager.Instance.RoomManager.SetRoomContext(parameters[0]);
    }

    void SwitchCam(string[] parameters) {
        Room room = WalkaroundManager.Instance.RoomManager.currentRoom;
        WalkaroundCamera cam = room.RoomCameras.Find(a => a.index == parameters[0]);
        WalkaroundManager.Instance.CameraManager.SetCamera(cam);
    }
    
    void SetLight(string[] parameters) {
        WalkaroundManager.Instance.RoomManager.SetLight(parameters[0], bool.Parse(parameters[1]));
    }
    
    void SpawnChar(string[] parameters) {
        GameObject npc = GameObject.Find(WalkaroundManager.Instance.Talkspriter.scriptToID[parameters[0]]);
        string node = parameters[1];
        npc.transform.position = 
            WalkaroundManager.Instance.RoomManager.currentRoom.Blocks[node].position;
    }

    void SpawnPlayer(string[] parameters) {
        string node = parameters[0];
        WalkaroundManager.Instance.sys.transform.position = 
            WalkaroundManager.Instance.RoomManager.currentRoom.Blocks[node].position;
        WalkaroundManager.Instance.sys.GetComponent<ObjectController>().ResetIdle();
    }

    void DespawnChar(string[] parameters) {
        GameObject npc = GameObject.Find(WalkaroundManager.Instance.Talkspriter.scriptToID[parameters[0]]);

        npc.transform.position = new Vector3(9999, 9999, 0);
    }

    void WalkChar(string[] parameters) {
        ObjectConfig npc = FindObjectsOfType<ObjectConfig>().First(a => a.IsControllable && a.ID == parameters[0]);

        npc.GetComponent<ObjectController>().OnMoveScripted(
            WalkaroundManager.Instance.RoomManager.currentRoom.Blocks[parameters[1]].position, 
            float.Parse(parameters[2]));
    }

    void WalkCharBlocking(string[] parameters, System.Action onComplete) {
        ObjectConfig npc = FindObjectsOfType<ObjectConfig>().First(a => a.IsControllable && a.ID == parameters[0]);
        DialogueContainer.SetActive(false);
        npc.GetComponent<ObjectController>().OnMoveScripted(
                WalkaroundManager.Instance.RoomManager.currentRoom.Blocks[parameters[1]].position, 
                float.Parse(parameters[2]),
                onComplete, DialogueContainer);
    }

    IEnumerator WaitForSFX(System.Action onComplete) {
        DialogueContainer.SetActive(false);
        while (SFXManager.Instance.GetAudio().isPlaying)
            yield return new WaitForEndOfFrame();
        DialogueContainer.SetActive(true);

        onComplete();
    }

    void SetCharExpression(string[] parameters) {
        WalkaroundManager.Instance.Talkspriter.SetEmotion(parameters[0], false, parameters[1]);
    }
    
    void SetFlag(string[] parameters) {
        string val = parameters[1];
        Debug.Log("Set gamestate flag " + parameters[0] + " to " + parameters[1]);
        
        if (val.Equals("True") || val.Equals("False")) {
            StoryModeGameManager.Instance.SetGamestateFlag(parameters[0], bool.Parse(parameters[1]));
        }
    }

    void PlaySFX(string[] parameters) {
        if (SFXManager.Instance) SFXManager.Instance.PlayAudio(parameters[0]);
    }

    void PlaySFXBlocking(string[] parameters, System.Action onComplete) {
        if (SFXManager.Instance) SFXManager.Instance.PlayAudio(parameters[0]);

        if (onComplete != null)
            StartCoroutine(WaitForSFX(onComplete));
    }
    void SwitchBGM(string[] parameters) {
        if (BGMManager.Instance) BGMManager.Instance.PlayAudio(parameters[0]);
    }

    void CutBGM(string[] parameters) {
        if (BGMManager.Instance) BGMManager.Instance.KillAudio();
    }

    void SwitchScene(string[] parameters) {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(parameters[0]));
        //queue up new node using parameters[1]
    }

    void CustomWait(string[] parameters, System.Action onComplete) {
        StartCoroutine(OnCustomWait(float.Parse(parameters[0]), onComplete));
    }

    IEnumerator OnCustomWait(float time, System.Action onComplete) {
        DialogueContainer.SetActive(false);
        yield return new WaitForSeconds(time);
        DialogueContainer.SetActive(true);
        onComplete();
    }

    void SetObjectProperties(string[] parameters) {
        GameObject npc = GameObject.Find(parameters[0]);
        ObjectConfig objc;
        if (npc.TryGetComponent<ObjectConfig>(out objc)) {
            switch (parameters[1]) {
                case "dialogue":
                    objc.IsDialogueTrigger = Boolean.Parse(parameters[2]);
                    break;
                case "interactable":
                    objc.IsInteractable = Boolean.Parse(parameters[2]);
                    break;
                case "cutscene":
                    objc.IsCutsceneTrigger = Boolean.Parse(parameters[2]);
                    break;
                case "key":
                    objc.SetKey(parameters[2]);
                    break;
            }
        }
    }

    void BuildSong(string[] parameters) {
        StoryModeGameManager.Instance.BuildSong(parameters[0]);
    }
    
    void SwitchSprite(string[] parameters) {
        WalkaroundManager.Instance.SwitchSprite(parameters[0],  parameters[1]);
    }
}