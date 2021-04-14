using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LoginScreenMenu : SimpleMenu<LoginScreenMenu> {
    public Animator loginAnim;

    delegate void AnimationComplete();
    private AnimationComplete animationComplete;

    public CinemachineVirtualCamera cam;
   
    public void OnLogin() {
        loginAnim.SetTrigger("Login");
        animationComplete += ModeSelectMenu.Show;
        MenuCameraManager.Instance.SwitchCamera(1);
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic() {
        yield return new WaitForSeconds(2f);
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
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
