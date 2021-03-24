using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class Room : SerializedMonoBehaviour {
    public List<WalkaroundCamera> RoomCameras;
    public Dictionary<string, Transform> Blocks;
    public Dictionary<string, LightController> LightControllers;
    
    private CinemachineBrain brain;
    
    public void SwitchCamera(string index) {
        WalkaroundManager.Instance.SetCamera(RoomCameras.Find(a => a.index == index));
    }

    public void SetLight(string index, bool active) {
        LightController lc = LightControllers[index];
        if (active) lc.EnableLamp(); else lc.DisableLamp();
    }
}