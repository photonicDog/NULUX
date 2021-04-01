using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LoginScreenMenu : SimpleMenu<LoginScreenMenu> {
    public Animator loginAnim;

    delegate void AnimationComplete();
    private AnimationComplete animationComplete;
   
    public void OnLogin() {
        loginAnim.SetTrigger("Login");
        animationComplete += ModeSelectMenu.Show;
    }

    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }
}