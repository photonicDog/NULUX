using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

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
    public CharacterTalksprites talksprites;
    public ObjectController npcBody;
    public List<IndexNode> nodes;

    public WalkaroundNPCState(string name, int state, CharacterTalksprites talksprites, ObjectController npcBody, List<IndexNode> nodes) {
        this.name = name;
        this.npcBody = npcBody;
        this.state = state;
        this.nodes = nodes;
        this.talksprites = talksprites;
    }
}

[Serializable]
public class IndexNode {
    public int index;
    public string node;
}