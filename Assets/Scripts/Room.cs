using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using WalkAround;

public class Room : SerializedMonoBehaviour {
    public List<WalkaroundCamera> RoomCameras;
    public Dictionary<string, Transform> Blocks;
    public Dictionary<string, LightController> LightControllers;
    public Dictionary<string, ObjectConfig> SwitchableObject;
    
    private CinemachineBrain brain;
    
    public void SetLight(string index, bool active) {
        LightController lc = LightControllers[index];
        if (active) lc.EnableLamp(); else lc.DisableLamp();
    }
}