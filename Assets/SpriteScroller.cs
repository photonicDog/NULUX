using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour {
    private Renderer spr;

    public Vector2 vel;
    
    private void Awake() {
        spr = GetComponent<Renderer>();
    }

    private void FixedUpdate() {
        spr.material.SetTextureOffset("_MainTex", vel*Time.time);
    }
}
