using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ScriptableObject {
    [CreateAssetMenu(fileName = "Gamestate", menuName = "ScriptableObjects/Gamestates/Gamestate", order = 1)]
    public class Gamestate : SerializedScriptableObject {

        public List<GamestateFlag> flags;
        public Dictionary<string, Track> tracks;

        public void Init(List<GamestateFlag> flags) {
            this.flags = new List<GamestateFlag>(flags);
        }

        public bool GetFlag(string index) {
            return flags.Find(a => a.id == index).flag;
        }

        public void SetFlag(string index, bool value) {
            flags.Find(a => a.id == index).flag = value;
        }
    }

    [Serializable]
    public class GamestateFlag {
        [HorizontalGroup("Flag")][HideLabel]public string id;
        [HorizontalGroup("Flag")][HideLabel]public bool flag;
    }
}