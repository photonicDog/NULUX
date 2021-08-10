using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WalkAround {
    public class RoomManager : SerializedMonoBehaviour {

        private Image fadeImage;
        private PlayerInput sys;
        private WalkaroundCameraManager CameraManager;
    
        [Header("Rooms")]
        public Dictionary<string, Room> rooms;
        [HideInInspector] public Room currentRoom;

        public void Initialize(Image fadeImage, PlayerInput sys, WalkaroundCameraManager CameraManager) {
            this.fadeImage = fadeImage;
            this.sys = sys;
            this.CameraManager = CameraManager;
        
            foreach (Room room in rooms.Values) {
                foreach (WalkaroundCamera vc in room.RoomCameras) {
                    vc.DeactivateCamera();
                }
            }
        }
    
        public void UseDoor(GameObject mover, Room room, WalkaroundCamera camera, Door door)
        {
            StartCoroutine(DoorAnimation(mover, room, camera, door));
        }

        private IEnumerator DoorAnimation(GameObject mover, Room room, WalkaroundCamera camera, Door door)
        {
            sys.DeactivateInput();
            fadeImage.color = Color.black - new Color(0f, 0f, 0f, 1f);
            while (fadeImage.color.a < 1f)
            {
                fadeImage.color += new Color(0f, 0f, 0f, 0.02f);
                yield return new WaitForEndOfFrame();
            }
            fadeImage.color = Color.black;
		
            Vector3 position = door.destinationPosn.position;
            mover.transform.position = position;
            SetRoomContext(room);
            CameraManager.SetCamera(camera);
            yield return new WaitForSeconds(0.2f);
		
            while (fadeImage.color.a > 0f)
            {
                fadeImage.color -= new Color(0f, 0f, 0f, 0.02f);
                yield return new WaitForEndOfFrame();
            }
            fadeImage.color = Color.black - new Color(0f, 0f, 0f, 1f);
            sys.ActivateInput();
        }

        public void SetRoomContext(string key) {
            SetRoomContext(rooms[key]);
            CameraManager.SetCamera(currentRoom.RoomCameras.First());
        }

        public void SetRoomContext(Room room) {
            currentRoom = room;
            CameraManager.SetCamera(currentRoom.RoomCameras.First());
        }

        public void SetLight(string s, bool parse) {
            throw new System.NotImplementedException("Light manager not implemented!");
        }
    }
}
