using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MusicSelectMenu : SimpleMenu<MusicSelectMenu> {

    public Animator selectAnim;
    
    delegate void AnimationComplete();
    private AnimationComplete animationComplete;
    
    public int selectedDifficulty;
    
    public override void OnBackPressed() {
        MenuCameraManager.Instance.SwitchCamera(1);
        selectAnim.SetTrigger("Exit");
        selectAnim.SetBool("ToSong", false);
        animationComplete += ModeSelectMenu.Show;
    }
    
    public void ExecuteOnAnimation() {
        animationComplete.Invoke();
        animationComplete = null;
    }

    public void LaunchFreePlay() {
        selectAnim.SetTrigger("Exit");
        selectAnim.SetBool("ToSong", true);
        BGMManager.Instance.KillAudio();
    }
}
