/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongUIManagerDeprecated : MonoBehaviour {

    public TextMeshProUGUI comboCounter;
    public TextMeshProUGUI floatingCombo;
    public Animation floatingComboPulse;

    public Image driveFill;
    public TextMeshProUGUI drivePercentage;
    public Image drivePacemaker;

    public TextMeshProUGUI songTitle;
    public TextMeshProUGUI scoreTop;
    public TextMeshProUGUI scoreBottom;
    
    private int trackedCombo;
    public void UpdateCombo(int combo) {
        if (combo != trackedCombo) {
            if (combo > 3) {
                floatingCombo.text = combo.ToString();
                floatingComboPulse.Rewind();
                floatingComboPulse.Play();
            }
            else {
                floatingCombo.text = "";
            }
        }

        comboCounter.text = combo.ToString("D4");
        
        trackedCombo = combo;
    }

    public void UpdateDrive(float percentage, float pacemaker) {
        driveFill.fillAmount = percentage;
        drivePercentage.text = (percentage*100f).ToString("F2") + "%";
        drivePacemaker.transform.position = 
            new Vector3(Mathf.Lerp(0, 1880, pacemaker), drivePacemaker.transform.position.y, 0);
    }

    public void UpdateSongAttributes(string title) {
        songTitle.text = title;
    }

    public void UpdateScore(int scr) {
        scoreTop.text = scr.ToString("D7").Substring(0, 3);
        scoreBottom.text = scr.ToString("D7").Substring(3, 4);
    }
}
*/
