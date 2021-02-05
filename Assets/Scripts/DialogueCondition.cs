using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCondition", menuName = "ScriptableObjects/Gamestates/DialogueCondition", order = 1)]
public class DialogueCondition : Gamestate {
    public string node;
    public int location;
    public bool played = false;
}
