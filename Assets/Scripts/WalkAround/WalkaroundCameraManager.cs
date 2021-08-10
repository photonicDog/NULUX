using Cinemachine;
using UnityEngine;

namespace WalkAround {
    public class WalkaroundCameraManager : MonoBehaviour {
    
        [Header("Camera")]
        public CinemachineBrain spriteCamBrain;
        public CinemachineBrain physicalBrain;
        public WalkaroundCamera currentCam;

        public void Initialize() {
        
        }
    
        public void SetCamera(WalkaroundCamera vc) {
            if (currentCam) currentCam.DeactivateCamera();
            vc.ActivateCamera();
            currentCam = vc;
        }        
    }
}
