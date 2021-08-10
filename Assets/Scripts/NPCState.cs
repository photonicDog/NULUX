using System.Collections;
using System.Collections.Generic;
using ScriptableObject;
using UnityEngine;

public class NPCState : MonoBehaviour {
    public int state;
    public List<IndexNode> nodes;

    public string GetDialogueByState() {
        return nodes.Find(a => a.index == state).node;
    }
}
