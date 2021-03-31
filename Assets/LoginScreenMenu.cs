using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LoginScreenMenu : SimpleMenu<LoginScreenMenu> {
    private Animator anim;

    delegate void AnimationComplete();
    private AnimationComplete animationComplete;
    
    
    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void OnLogin() {
        anim.SetTrigger("Login");
        animationComplete += ModeSelectMenu.Show;
    }

    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }
}
