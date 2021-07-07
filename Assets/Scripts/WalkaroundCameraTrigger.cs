using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WalkaroundCameraTrigger : MonoBehaviour {
    
    private WalkaroundCamera storedCamera;
    public WalkaroundCamera transitionCam;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            storedCamera = WalkaroundManager.Instance.CameraManager.currentCam;
            if (storedCamera != transitionCam) {
                WalkaroundManager.Instance.CameraManager.SetCamera(transitionCam);
            }
        }
    }
}
