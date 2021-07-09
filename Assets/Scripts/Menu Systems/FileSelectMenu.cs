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
        BGMManager.Instance.KillAudio();
    }

    public override void OnBackPressed() {
        MenuCameraManager.Instance.SwitchCamera(1);
        fileAnim.SetTrigger("Exit");
        fileAnim.SetBool("ToStory", false);
        WindowStateManager.Instance.CurrentScreen = 1;
        animationComplete += ModeSelectMenu.Show;
    }
    
    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }
}
