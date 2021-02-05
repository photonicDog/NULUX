using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneBG : MonoBehaviour {

    private Image img;

    public Sprite defaultSprite;

    void Awake() {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    public void SetBGInstant(Sprite s) {
        img.enabled = true;
        img.color = Color.white;
        img.sprite = s;
    }

    public void SetBGInstant(Color c) {
        img.enabled = true;
        img.sprite = defaultSprite;
        img.color = c;
    }

    public void SetBG(Color c, float time, System.Action onComplete, GameObject go) {
        StartCoroutine(DoSetBG(c, time, onComplete, go));
    }

    public void SetBG(Sprite s, float time, CutsceneBG pushTo, System.Action onComplete, GameObject go) {
        StartCoroutine(DoSetBG(s, time, pushTo, onComplete, go));
    }

    IEnumerator DoSetBG(Color c, float time, System.Action onComplete, GameObject go) {
        go.SetActive(false);
        img.enabled = true;
        img.sprite = defaultSprite;
        Color orig = img.color;
        float elapsed = 0;
        while (img.color != c) {
            img.color = Color.Lerp(orig, c, elapsed / time);
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }
        img.color = c;
        go.SetActive(true);
        onComplete();
    }
    
    IEnumerator DoSetBG(Sprite s, float time, CutsceneBG pushTo, System.Action onComplete, GameObject go) {
        go.SetActive(false);
        img.enabled = true;
        img.color = Color.white - new Color(0,0,0,1);
        img.sprite = s;
        float elapsed = 0;
        while (img.color != Color.white) {
            img.color = Color.Lerp(Color.clear, Color.white, elapsed / time);
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }
        
        pushTo.SetBGInstant(s);
        img.enabled = false;
        go.SetActive(true);
        onComplete();
    }

    public void ClearBG() {
        img.enabled = false;
    }
}
