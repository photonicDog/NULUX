using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour {
    public List<CinemachineVirtualCamera> roomCameras;
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
        roomCameras[index].gameObject.SetActive(true);
    }
}
