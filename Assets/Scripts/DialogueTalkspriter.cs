using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTalkspriter : MonoBehaviour {

    [SerializeField]
    private Animator anim = default;

    [SerializeField]
    private List<CharacterTalksprites> talksprites = default;

    public Image talkspr;

    public GameObject nameplate;

    [SerializeField] private float fps = 4;


    public AnimationClip blankClip;
    public AnimationClip defaultClip;

    private AnimationClip clip;
    private AnimatorOverrideController animatorOverride;
    
    // Start is called before the first frame update
    void Awake() {
        StopAnimation();
    }

    public void SetSprite(string name, string key) {
        if (name == "NARRATOR") {
            clip = blankClip;
            nameplate.SetActive(false);
        } else {
            nameplate.SetActive(true);
            if (talksprites.Exists(a => a.Nametag.Equals(name))) {
                CharacterTalksprites t = talksprites.Find(a => a.Nametag.Equals(name));
                if (t) {
                    try {
                        clip = t.talkspriteList[key];
                    }
                    catch (KeyNotFoundException) {
                        clip = t.talkspriteList["NORMAL"];
                    }
                }
                else {
                    clip = defaultClip;
                }
            }
            else {
                clip = blankClip;
            }
        }

        anim.Play(clip.name);
    }

    public void StartAnimation() {
        anim.enabled = true;
    }
    
    public void StopAnimation() {
        anim.enabled = false;
    }

    public void ChangeSpeed(string code, float fps) {
        this.fps = fps;
    }

    public void ImportAllTalksprites(List<CharacterTalksprites> ts) {
        talksprites.AddRange(ts);
    }
}
