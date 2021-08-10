using ScriptableObject;
using UnityEngine;

namespace Dialogue_System {
    [CreateAssetMenu(fileName = "DialogueCondition", menuName = "ScriptableObjects/Gamestates/DialogueCondition", order = 1)]
    public class DialogueCondition : Gamestate {
        public string node;
        public int location;
        public bool played = false;
    }
}
