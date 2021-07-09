using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTalkspriterSong : DialogueTalkspriter {

    public Dictionary<string, string> scriptToID;
    public Dictionary<string, string> scriptToNameplate;
    public Dictionary<string, TalkspriteController> inSongTalksprites;

    private TalkspriteController current;

    public void Awake() {
        foreach (TalkspriteController tc in inSongTalksprites.Values) {
            tc.gameObject.SetActive(false);
        }
    }
    public override void SetEmotion(string id, bool script, string emotion) {
        TalkspriteController currentTalksprite = inSongTalksprites[script ? scriptToID[id] : id];
        
        ManageCurrentSprite(currentTalksprite);

        currentTalksprite.SetAnimation(emotion);
    }

    public override void SetState(string id, bool walk, bool set) {
        TalkspriteController currentTalksprite = inSongTalksprites[scriptToID[id]];
        
        ManageCurrentSprite(currentTalksprite);
        
        currentTalksprite.SetState(walk, set);
    }

    public override string GetNameplate(string scriptPlate) {
        return scriptToNameplate[scriptPlate];
    }

    public override void Bubble(string id, bool display) {

    }

    private void ManageCurrentSprite(TalkspriteController currentTalksprite) {
        if (current != null && currentTalksprite != current) {
            current.gameObject.SetActive(false);
            current = currentTalksprite;
            current.gameObject.SetActive(true);
        }
        else if (current == null) {
            current = currentTalksprite;
            current.gameObject.SetActive(true);
        }
    }
}
