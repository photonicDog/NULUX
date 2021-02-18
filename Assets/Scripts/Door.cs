using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.WalkAround.Objects.Implementations;
using UnityEngine;

public class Door : MonoBehaviour {
    public Transform destinationPosn;
    public string room;
    public string cameraIndex;

    public void MoveThroughDoor(ObjectConfig traveler) {
        WalkaroundManager.Instance.UseDoor(traveler.gameObject, room, cameraIndex, this);
    }
}
