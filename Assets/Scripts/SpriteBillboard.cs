using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour {
    private CinemachineBrain brain;

    IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        brain = WalkaroundManager.Instance.spriteCamBrain;
    }

    void Update() 
    {
        if (brain)
        transform.LookAt(WalkaroundManager.Instance.currentCam.transform.position, Vector3.up);
    }
}