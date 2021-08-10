using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Menu_Systems {
    public class LoginScreenMenu : SimpleMenu<LoginScreenMenu> {
        public Animator loginAnim;

        delegate void AnimationComplete();
        private AnimationComplete animationComplete;

        public CinemachineVirtualCamera cam;
   
        public void OnLogin() {
            loginAnim.SetTrigger("Login");
            animationComplete += ModeSelectMenu.Show;
            MenuCameraManager.Instance.SwitchCamera(1);
            BGMManager.Instance.PlayAudio("mus_title");
        }

        public void ExecuteOnAnimation() {
            animationComplete.Invoke();
            animationComplete = null;
        }
    
        public override void OnBackPressed() {
            StartCoroutine(DoEndGame());
        }

        private IEnumerator DoEndGame()
        {
            yield return new WaitForSeconds(0.5f);
            Application.Quit();
        }
    }
}
