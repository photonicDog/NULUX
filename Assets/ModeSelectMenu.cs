using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ModeSelectMenu : SimpleMenu<ModeSelectMenu>
{
    delegate void AnimationComplete();
    private AnimationComplete animationComplete;

    public Animator parentAnim;
    
    public void OnStoryPressed() {
        parentAnim.SetTrigger("Exit");
        animationComplete += FileSelectMenu.Show;
    }

    public void OnFreePlayPressed() {
        
    }

    public void OnOptionsPressed() {
        
    }

    public void OnQuitPressed() {
        
    }
    
    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }
}
