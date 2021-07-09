
    /*
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SongLaunchCarrierDeprecated : MonoBehaviour {

        public static SongLaunchCarrierDeprecated Instance;
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

            GameObject.Find("SongManager").GetComponent<SongManagerDeprecated>().SetCurrentTrack(currentTrack);
            Destroy(gameObject);
        }
    }
    */
