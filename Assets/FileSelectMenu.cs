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
        fileAnim.SetTrigger("Login");
    }
}
