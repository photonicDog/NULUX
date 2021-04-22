using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MainMenuBarController : MonoBehaviour {
    public TextMeshProUGUI clock;

    private DateTime time;
    public MenuManager mm;

    private void Awake() {
        mm.OpenMenu(mm.MenuScreens[WindowStateManager.Instance.CurrentScreen]);
    }

    private void Update() {
        time = DateTime.Now;
        clock.text = time.ToShortTimeString();
    }
}
