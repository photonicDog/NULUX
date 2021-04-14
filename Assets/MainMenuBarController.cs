using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuBarController : MonoBehaviour {
    public TextMeshProUGUI clock;

    private DateTime time;

    private void Update() {
        time = DateTime.Now;
        clock.text = time.ToShortTimeString();
    }
}
