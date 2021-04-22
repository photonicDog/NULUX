using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TalkspriteController : SerializedMonoBehaviour {

    public Dictionary<string, int> animationDictionary;
    
    [ShowInInspector] public Animator anim;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimation(string animation) {
        anim.SetInteger("Emotion", animationDictionary[animation]);
    }

    public void SetState(bool walk, bool set) {
        if (walk) {
            anim.SetTrigger(set?"Walk":"NoWalk"); 
        }
        else {
            anim.SetTrigger(set?"Talk":"NoTalk"); 
        }
        
    }
}
