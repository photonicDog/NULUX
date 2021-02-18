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
        anim.SetInteger(0, animationDictionary[animation]);
    }

    public void SetState(bool walk, bool set) {
        anim.SetBool(walk?1:2, set);
    }
}
