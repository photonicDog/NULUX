using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WalkAround.Objects.Implementations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTalkspriter : SerializedMonoBehaviour {

    private List<ObjectConfig> talkspritables;

    public Dictionary<string, string> scriptToID;
    public Dictionary<string, string> scriptToNameplate;
    
    void Awake() {
        talkspritables = FindObjectsOfType<ObjectConfig>().Where(a => a.IsTalkspritable).ToList();
    }

    public void SetEmotion(string id, string emotion) {
        ObjectConfig ts = talkspritables.Find(a => a.ID == scriptToID[id]);
        if (ts) ts.talkspriteController.SetAnimation(emotion);
    }

    public void SetState(string id, bool walk, bool set) {
        ObjectConfig ts = talkspritables.Find(a => a.ID == scriptToID[id]);
        if (ts) ts.talkspriteController.SetState(walk, set);
    }

    public string GetNameplate(string scriptPlate) {
        return scriptToNameplate[scriptPlate];
    }
}
