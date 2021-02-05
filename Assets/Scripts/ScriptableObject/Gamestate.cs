using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Gamestate", menuName = "ScriptableObjects/Gamestates/Gamestate", order = 1)]
public class Gamestate : SerializedScriptableObject {
    
    public Dictionary<string, bool> booleanFlags;
    public Dictionary<string, int> integerFlags;

    public Dictionary<string, Track> tracks;

    public void Init(Dictionary<string, bool> booleanFlags, Dictionary<string, int> integerFlags) {
        this.booleanFlags = new Dictionary<string, bool>(booleanFlags);
        this.integerFlags = new Dictionary<string, int>(integerFlags);
    }
}
