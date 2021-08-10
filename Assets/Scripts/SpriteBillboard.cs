using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using WalkAround;

public class SpriteBillboard : MonoBehaviour {
    private CinemachineBrain brain;

    IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        brain = WalkaroundManager.Instance.CameraManager.spriteCamBrain;
    }

    void Update() {
        Vector3 camPos = WalkaroundManager.Instance.CameraManager.currentCam.transform.position;
        if (brain)
        transform.LookAt(camPos, Vector3.up);
    }
}