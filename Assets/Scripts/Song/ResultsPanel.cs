using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class ResultsPanel : SerializedMonoBehaviour {

    public TextMeshProUGUI scoreResult;
    public TextMeshProUGUI rankResult;
    public GameObject resultsScreen;
    public GameObject continueButton;
    public EventSystem ui;
    public SongManager sm;

    public Dictionary<ScoringHeuristic, TextMeshProUGUI> resultText;
    
    private Dictionary<ScoringHeuristic, int> noteResults;
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
        noteResults = SongManager.Instance.noteResults;
        scoreResult.text = score.ToString("D7");

        foreach (var res in noteResults) {
            resultText[res.Key].text = res.Value.ToString();
        }
        
        if (score < 700000) rankResult.text = "D";
        if (score > 700000) rankResult.text = "C";
        if (score > 800000) rankResult.text = "B";
        if (score > 900000) rankResult.text = "A";
        if (score > 925000) rankResult.text = "AA";
        if (score > 950000) rankResult.text = "S";
        if (score > 975000) rankResult.text = "SS";
        if (score > 999999) rankResult.text = "X";
        
        ui.SetSelectedGameObject(continueButton);
    }

    public void LeaveSong() {
        if (sm.isStoryMode) {
            foreach (GamestateFlag kv in sm.currentTrack.postSongFlags) {
                StoryModeGameManager.Instance.SetGamestateFlag(kv.id, kv.flag);
            }
            
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(2));
        }
        else {
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(1));
        }
    }
}
