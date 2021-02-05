using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkspriteGroup", menuName = "ScriptableObjects/Character Talksprites", order = 1)]
public class CharacterTalksprites : SerializedScriptableObject {
    public string Nametag;
    public Dictionary<string, AnimationClip> talkspriteList;
}
