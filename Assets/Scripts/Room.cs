using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DraggablePoint : PropertyAttribute {}

public class Room : MonoBehaviour {
    public List<CinemachineVirtualCamera> RoomCameras;
    public List<BlockingPosition> Blocks;
    public List<LightController> LightControllers;
    
    private CinemachineBrain brain;
    
    // Start is called before the first frame update
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToRoom(int index) {
        brain.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        RoomCameras[index].gameObject.SetActive(true);
    }
}

[Serializable]
public class BlockingPosition {
    public string id;
    public Transform position;
}