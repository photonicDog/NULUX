using Song.Types;
using UnityEngine;

namespace Song {
    [CreateAssetMenu(fileName = "SongSettings", menuName = "Song", order = 0)] 
    public class SongSettings : UnityEngine.ScriptableObject {
        public JudgementWindow judgementWindow;
    }
}
