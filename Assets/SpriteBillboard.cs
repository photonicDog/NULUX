using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour {
    private CinemachineBrain brain;

    IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void Update() 
    {
        if (brain)
        transform.LookAt(brain.ActiveVirtualCamera.VirtualCameraGameObject.transform.position, Vector3.up);
    }
}