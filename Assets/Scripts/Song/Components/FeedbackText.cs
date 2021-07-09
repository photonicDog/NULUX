using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackText : MonoBehaviour {

    private GameObject currentText;
    // Start is called before the first frame update

    public void DisplayFeedback(GameObject go, Transform source) {
        Destroy(currentText);

        Vector3 position = source.position;
        currentText = Instantiate(go, position, Quaternion.identity, transform);
        currentText.transform.position = position + (Vector3.up * 0.5f);
    }

    public void ResetFeedback() {
        Destroy(currentText);
    }
}
