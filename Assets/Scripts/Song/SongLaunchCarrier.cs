
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SongLaunchCarrier : MonoBehaviour {

        public static SongLaunchCarrier Instance;
        public Track currentTrack;
        
        void Awake() {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void Execute(Track t) {
            currentTrack = t;
            StartCoroutine(SongFlywrench());
        }

        IEnumerator SongFlywrench() {
            while (!GameObject.Find("SongManager")) {
                yield return new WaitForEndOfFrame();
            }

            GameObject.Find("SongManager").GetComponent<SongManager>().SetCurrentTrack(currentTrack);
            Destroy(gameObject);
        }
    }
