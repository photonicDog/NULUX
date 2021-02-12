using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WalkAround.Objects.Implementations;
using UnityEngine;

public class Door : MonoBehaviour {
    public Vector3 destinationPosn;
    public Room room;
    public int cameraIndex;
    private Animator doorAnimation;

    void Start() {
        doorAnimation = GetComponent<Animator>();
        doorAnimation.SetBool("open", false);
    }

    void OpenDoor() {
        
    }

    void CloseDoor() {
        
    }

    public void MoveThroughDoor(ObjectConfig traveler) {
        traveler.transform.position = destinationPosn;
        
        if (traveler.IsPlayer)
        room.MoveToRoom(cameraIndex);
    }
}
