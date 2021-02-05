using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioAssetKey", menuName = "ScriptableObjects/Asset Management/Audio Asset Key", order = 1)]
public class AudioAssetKey : SerializedScriptableObject {
    public Dictionary<string, LoopableAudio> audioDict;
}

[Serializable]
public class LoopableAudio {
    public AudioClip audio;
    public AudioClip start;
}
