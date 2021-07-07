using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpriteSwitcher : SerializedMonoBehaviour {
    private SpriteRenderer renderer;

    public Dictionary<string, Sprite> sprites;
    // Start is called before the first frame update
    void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchSprite(string index) {
        renderer.sprite = sprites[index];
    }
}
