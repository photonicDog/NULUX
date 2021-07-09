using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempFreePlayManager : MonoBehaviour
{
    public static TempFreePlayManager Instance;
    public Track prototypeTrack;
    
    private void Awake() {
        Object.DontDestroyOnLoad(base.gameObject);
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.Log("Multiple FreePlayManager in the scene!");
            Object.Destroy(this.gameObject);
        }
    }
    
    public void FreePlaySelect() {
        StartCoroutine(OnFreePlaySelect());
    }

    private IEnumerator OnFreePlaySelect() {
        while (!GameObject.Find("SongManager")) {
            yield return new WaitForEndOfFrame();
        }

        GameObject.Find("SongManager").GetComponent<SongManagerDeprecated>().SetCurrentTrack(prototypeTrack);
    }
}
