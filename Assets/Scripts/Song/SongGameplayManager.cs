using System;
using System.Collections.Generic;
using Song.Types;
using UnityEngine;

namespace Song {
    public class SongGameplayManager : MonoBehaviour {
        public SongSettings settings;

        private static SongGameplayManager _instance;
        public static SongGameplayManager Instance { get { return _instance;  } }

        private Queue<HitEvent> hitEventQueue = new Queue<HitEvent>();
        /// <summary>
        /// Activates all manager initialization methods
        /// </summary>
        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            } else {
                _instance = this;
            }
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void AddToHitEventQueue(HitEvent hd) {
            hitEventQueue.Enqueue(hd);
        }
    }
}
