using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class ResultsPanel : MonoBehaviour {

    public TextMeshProUGUI scoreResult;
    public TextMeshProUGUI rankResult;
    public GameObject resultsScreen;
    public GameObject continueButton;
    public EventSystem ui;
    public SongManager sm;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score) {
        resultsScreen.SetActive(true);
        scoreResult.text = score.ToString("D7");
        if (score < 600000) rankResult.text = "F";
        if (score > 600000) rankResult.text = "D";
        if (score > 700000) rankResult.text = "C";
        if (score > 800000) rankResult.text = "B";
        if (score > 900000) rankResult.text = "A";
        if (score > 950000) rankResult.text = "S";

        ui.SetSelectedGameObject(continueButton);
    }

    public void LeaveSong() {
        if (sm.isStoryMode) {
            foreach (KeyValuePair<string, bool> kv in sm.currentTrack.postSongFlagsBool) {
                StoryModeGameManager.Instance.SetGamestateFlag(kv.Key, kv.Value);
            }
            foreach (KeyValuePair<string, int> kv in sm.currentTrack.postSongFlagsInt) {
                StoryModeGameManager.Instance.SetGamestateFlag(kv.Key, kv.Value);
            }
            
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(2));
        }
        else {
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
        }
    }
}
