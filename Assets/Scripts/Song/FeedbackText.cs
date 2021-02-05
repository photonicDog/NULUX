using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackText : MonoBehaviour {

    public List<GameObject> feedback;
    
    // Start is called before the first frame update

    public void DisplayFeedback(int i, Transform source) {
        StopCoroutine("TextFade");
        
        foreach (GameObject g in feedback) {
            g.SetActive(false);
        }

        transform.position = source.position + (Vector3.up * 0.5f);
        feedback[i].SetActive(true);

        StartCoroutine(TextFade(500f));
    }

    public void ResetFeedback() {
        foreach (GameObject g in feedback) {
            g.SetActive(false);
        }
    }

    IEnumerator TextFade(float speedInMS) {
        yield return new WaitForSeconds((speedInMS * 0.001f));
        ResetFeedback();
    }
}
