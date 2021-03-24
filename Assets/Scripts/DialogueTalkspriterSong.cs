using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WalkAround.Objects.Implementations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTalkspriterSong : DialogueTalkspriter {

    private List<ObjectConfig> talkspritables;

    public Dictionary<string, string> scriptToID;
    public Dictionary<string, string> scriptToNameplate;
    public Dictionary<string, Animator> inSongTalksprites;
    
    void Awake() {
        talkspritables = FindObjectsOfType<ObjectConfig>().Where(a => a.IsTalkspritable).ToList();
    }

    public override void SetEmotion(string id, bool script, string emotion) {
        ObjectConfig ts = talkspritables.Find(a => a.ID.Equals(script?scriptToID[id]:id));
        if (ts) ts.talkspriteController.SetAnimation(emotion);
    }

    public override void SetState(string id, bool walk, bool set) {
        ObjectConfig ts = talkspritables.Find(a => a.ID == scriptToID[id]);
        if (ts) ts.talkspriteController.SetState(walk, set);
    }

    public override string GetNameplate(string scriptPlate) {
        return scriptToNameplate[scriptPlate];
    }

    public override void Bubble(string id, bool display) {
        ObjectConfig ts = talkspritables.Find(a => a.ID == scriptToID[id]);
        if (!ts) return;
        if (ts.HasInteractBubble) {
            ts.interactBubble.gameObject.SetActive(false);
        }
        if (ts.HasTalkBubble || ts.IsPlayer) {  
            ts.talkBubble.gameObject.SetActive(display);
        }
    }
}
