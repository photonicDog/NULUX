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
        parentAnim.SetTrigger("Exit");
        animationComplete += MusicSelectMenu.Show;
    }

    public void OnOptionsPressed() {
        
    }

    public void OnQuitPressed() {
        parentAnim.SetTrigger("Exit");
        animationComplete += FileSelectMenu.Show;
    }

    public override void OnBackPressed() {
        StartCoroutine(DoEndGame());
    }

    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }

    private IEnumerator DoEndGame()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
