using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ScriptableObject {
    [CreateAssetMenu(fileName = "WalkaroundState", menuName = "ScriptableObjects/Gamestates/NPC Walkaround State", order = 1)]
    public class WalkaroundNPCScenarioState : SerializedScriptableObject {
    
        public List<WalkaroundNPCState> scenarioNPCS;

        public void ChangeNPCState(string npc, int state) {
            scenarioNPCS.Find(a => a.name.Equals(npc)).state = state;
        }

        public string GetCurrentNode(string npc) {
            WalkaroundNPCState s = scenarioNPCS.Find(a => a.name.Equals(npc));

            return s.nodes.Find(a => a.index == s.state).node;
        }
    }

    [Serializable]
    public class WalkaroundNPCState {
        public string name;
        public int state;
        public List<IndexNode> nodes;

        public WalkaroundNPCState(string name, int state, List<IndexNode> nodes) {
            this.name = name;
            this.state = state;
            this.nodes = nodes;
        }
    }

    [Serializable]
    public class IndexNode {
        public int index;
        public string node;
    }
}