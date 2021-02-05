using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneFG : MonoBehaviour {

    private Image img;

    public Sprite defaultSprite;

    void Awake() {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    public void SetFG(Sprite s, float x, float y, float w, float h) {
        img.enabled = true;
        img.color = Color.white;
        img.sprite = s;
        transform.position = new Vector2(x * 1920, y * 1080);
    }

    public void ClearFG() {
        img.enabled = false;
    }
}
