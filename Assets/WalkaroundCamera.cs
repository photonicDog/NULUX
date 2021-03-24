using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WalkaroundCamera : MonoBehaviour {
    private CinemachineVirtualCamera cam;
    public string index;
    
    // Start is called before the first frame update
    void Awake() {
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.enabled = false;
    }

    public void ActivateCamera() {
        cam.enabled = true;
    }

    public void DeactivateCamera() {
        cam.enabled = false;
    }
}
