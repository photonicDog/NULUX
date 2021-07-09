using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour {

    public SongManagerDeprecated sm;

    public TextMeshProUGUI offsetText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        offsetText.text = "offset: " + (Conductor.Instance.offset) + " ms";

    }
}
