using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour {

    public TextMeshProUGUI offsetText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        offsetText.text = SettingsManager.Instance.userOffset.ToString() + " ms";
    }

    public void ChangeOffset(int i) {
        SettingsManager.Instance.userOffset += i;
    }
}
