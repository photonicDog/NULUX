using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class ResultsPanel : SerializedMonoBehaviour {

    public TextMeshProUGUI scoreResult;
    public TextMeshProUGUI rankResult;
    public GameObject resultsScreen;
    public GameObject continueButton;
    public EventSystem ui;
    public SongManager sm;
    public UILineRenderer offsetLine;
    public UILineRenderer timeLine;
    public TextMeshProUGUI offsetText;

    public float endBeat;

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
        ShowScore(score);
        ShowOffsetsFrequency(SongManager.Instance.recordedData);
        ShowOffsetsTime(SongManager.Instance.recordedData);
        
        ui.SetSelectedGameObject(continueButton);
    }

    private void ShowScore(int score) {    
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
    }

    private void ShowOffsetsFrequency(List<HitData> data) {
        int maxCt = 0;
        float pointCt = 500;
        List<Vector2> pointList = new List<Vector2>();
        int ct = 0;

        for (int i = 0; i <= pointCt; i++) {
            ct = data.Count(a => a.offset > Mathf.Lerp(-90, 90, Mathf.Max(0, i-(pointCt/10)) / pointCt) 
                              && a.offset < Mathf.Lerp(-90, 90, Mathf.Min(pointCt, i+(pointCt/10)) / pointCt));
            if (ct > maxCt) maxCt = ct;
            pointList.Add(new Vector2(i, ct));
        }

        List<Vector2> transformed = new List<Vector2>();

        foreach (Vector2 a in pointList) {
            transformed.Add(new Vector2(a.x / pointCt, a.y / maxCt));
        }

        offsetLine.Points = transformed.ToArray();

        offsetText.text = "AVERAGE OFFSET: " + data.Average(a => a.offset);
    }

    private void ShowOffsetsTime(List<HitData> data) {
        float pointCt = 300;
        List<Vector2> transformed = new List<Vector2>();
        float regionAverage = 0;
        for (int i = 0; i <= pointCt; i++) {
            try {
                regionAverage =
                    data.FindAll(a =>
                            a.time > Mathf.Lerp(0, endBeat, i / pointCt) &&
                            a.time <= Mathf.Lerp(0, endBeat, i + 1 / pointCt))
                        .Where(a => Math.Abs(a.offset) != 0f)
                        .Average(a => a.offset);
            }
            catch (Exception) {
                regionAverage = 0;
            };
            transformed.Add(new Vector2(i / pointCt, regionAverage / 90f + 0.5f));
        }
        
        timeLine.Points = transformed.ToArray();
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
