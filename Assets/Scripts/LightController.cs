using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightController : MonoBehaviour {
    [SerializeField] private UnityEvent OnEnableLamp;
    [SerializeField] private UnityEvent OnDisableLamp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableLamp() {
        OnEnableLamp.Invoke();
    }

    void DisableLamp() {
        OnDisableLamp.Invoke();
    }
}
