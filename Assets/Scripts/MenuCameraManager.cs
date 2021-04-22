using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraManager : MonoBehaviour {

    public Animator cameraAnim;

    public static MenuCameraManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void SwitchCamera(int screen) {
        cameraAnim.SetInteger("Screen", screen);
    }
    
}
