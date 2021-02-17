using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance;

    public int userOffset;
    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
