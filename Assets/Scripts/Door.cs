using System.Collections;
using Assets.Scripts.WalkAround.Objects.Implementations;
using Sirenix.OdinInspector;
using UnityEngine;

public class Door : MonoBehaviour {
    [ValueDropdown("GetAllRooms")]
    public Room room;
    
    [ValueDropdown("GetAllCamerasInRoom")]
    public WalkaroundCamera camera;
    
    [ValueDropdown("GetAllBlocks")]
    public Transform destinationPosn;


    public void MoveThroughDoor(ObjectConfig traveler) {
        WalkaroundManager.Instance.UseDoor(traveler.gameObject, room, camera, this);
    }

    private IEnumerable GetAllRooms() {
        return Object.FindObjectsOfType(typeof(Room));
    }

    private IEnumerable GetAllCamerasInRoom() {
        if (room) return room.transform.GetComponentsInChildren(typeof(WalkaroundCamera));
        return null;
    }

    private IEnumerable GetAllBlocks() {
        if (room) return room.transform.Find("Blocks");
        return null;
    }
}
