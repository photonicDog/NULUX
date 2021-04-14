using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class FileSelectMenu : SimpleMenu<FileSelectMenu>
{
    public Animator fileAnim;

    delegate void AnimationComplete();
    private AnimationComplete animationComplete;
    public void OnFile() {
        fileAnim.SetTrigger("Exit");
        fileAnim.SetBool("ToStory", true);
    }

    public override void OnBackPressed() {
        MenuCameraManager.Instance.SwitchCamera(1);
        fileAnim.SetTrigger("Exit");
        fileAnim.SetBool("ToStory", false);
        animationComplete += ModeSelectMenu.Show;
    }
    
    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }
}
