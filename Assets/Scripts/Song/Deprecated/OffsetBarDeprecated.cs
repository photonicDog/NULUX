/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OffsetBarDeprecated : MonoBehaviour {
    
    [SerializeField] private GameObject mark = default;
    [SerializeField] private GameObject arrow = default;
    private List<float> rollingOffsetList;
    
    // Start is called before the first frame update
    void Start()
    {
        rollingOffsetList = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MakeMark(float offset, ScoringHeuristic score) {
        GameObject newMark = Instantiate(mark, transform);
        newMark.transform.localPosition = transform.localPosition + new Vector3(0, -offset, 0);
        
        rollingOffsetList.Add(offset);
        if (rollingOffsetList.Count > 30) rollingOffsetList.RemoveAt(0);
        
        float avg = rollingOffsetList.Average();
        Vector3 position = arrow.transform.localPosition;
        position = new Vector3(position.x, avg, position.z);
        arrow.transform.localPosition = position;

        StartCoroutine(FadeMark(newMark.GetComponent<Image>(), score));
    }

    IEnumerator FadeMark(Image markImage, ScoringHeuristic score) {
        Color c;

        switch (score) {
            case ScoringHeuristic.STELLAR:
                c = Color.magenta;
                break;
            case ScoringHeuristic.PERFECT:
                c = Color.cyan;
                break;
            case ScoringHeuristic.GREAT:
                c = Color.green;
                break;
            case ScoringHeuristic.GOOD:
                c = Color.yellow;
                break;
            case ScoringHeuristic.MISS:
                c = Color.red;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(score), score, null);
        }

        markImage.color = c;
        
        while (markImage.color.a > 0.02f && markImage.color.r < 1) {
            markImage.color -= new Color(-0.02f, -0.02f, -0.02f, 0.02f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
*/
