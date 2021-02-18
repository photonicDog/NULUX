using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class DraggablePoint : PropertyAttribute {}

public class Room : SerializedMonoBehaviour {
    public Dictionary<string, CinemachineVirtualCamera> RoomCameras;
    public Dictionary<string, BlockingPosition> Blocks;
    public Dictionary<string, LightController> LightControllers;
    
    private CinemachineBrain brain;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToRoom(string index) {
SwitchCamera(index);
    }

    public void SwitchCamera(string index) {
        WalkaroundManager.Instance.SetCamera(RoomCameras[index]);
    }

    public void SetLight(string index, bool active) {
        LightController lc = LightControllers[index];
        if (active) lc.EnableLamp(); else lc.DisableLamp();
    }
}

[Serializable]
public class BlockingPosition {
    public string id;
    public Transform position;
}