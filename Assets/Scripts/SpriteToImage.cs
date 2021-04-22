using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpriteToImage : MonoBehaviour {
    private SpriteRenderer spr;
    private Image img;

    void Awake() {
        spr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
    }
    void Update() {
        img.sprite = spr.sprite;
    }
}
