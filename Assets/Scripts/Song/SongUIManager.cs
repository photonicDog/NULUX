using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongUIManager : MonoBehaviour {

    public TextMeshProUGUI comboCounter;

    public SongManager sm;

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        comboCounter.text = sm.combo.ToString("D4");
    }
}
