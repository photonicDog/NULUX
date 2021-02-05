using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : SerializedMonoBehaviour {

    private InputSystemUIInputModule isuim;
    private EventSystem ev;
    public Button firstSelected;
    public Animator uianimator;
    public Animator bgAnimator;
    public int sceneTarget;

    public Dictionary<int, Button> sceneButtonMapping;

    public int screen = 0;

    
    // Start is called before the first frame update
    void Start() {
        BGMManager.Instance.PlayAudio("mus_title");
        isuim = GetComponent<InputSystemUIInputModule>();
        ev = GetComponent<EventSystem>();
        ev.SetSelectedGameObject(firstSelected.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectButton(TextMeshProUGUI text) {
        text.text = "<#FFAEC9>>" + text.text + "</color>";
        SFXManager.Instance.PlayAudio("sfx_sel");
    }

    public void DeselectButton(TextMeshProUGUI text) {
        string originalString = text.text;
        if (originalString.Contains("<#FFAEC9>>")) {
            string strippedString = text.text.Substring(10, text.text.Length - 10);
            strippedString = strippedString.Substring(0, strippedString.Length - 8);
            text.text = strippedString;
        }
    }
    public void MoveToMenu(int to) {
        SFXManager.Instance.PlayAudio("sfx_confirm");
        var currentScreen = uianimator.GetInteger("screen");
        uianimator.SetInteger("screen", to);
        screen = to;
        StartCoroutine(ResetCursor());
    }

    IEnumerator ResetCursor() {
        EventTrigger et;
        float timeout = 3000;
        if (!sceneButtonMapping.ContainsKey(screen)) yield break;
        GameObject selection = sceneButtonMapping[screen].gameObject;
        if (selection != null) {
            while (selection.activeSelf == false) {
                timeout--;
                if (timeout <= 0) yield break;
                yield return new WaitForEndOfFrame();
            }
            ev.SetSelectedGameObject(selection);
            if (selection.TryGetComponent(out et)) {
                et.OnDeselect(new BaseEventData(ev));
                et.OnSelect(new BaseEventData(ev));
            }
        }
    }

    public void MoveToScene(int to) {
        sceneTarget = to;
        MoveToMenu(3);
        BGMManager.Instance.KillAudio();
    }

    public void EndGame() {
        WindowStateManager.Instance.EndGame();
    }
}
