using System.Collections;
using System.Collections.Generic;
using ScriptableObject;
using TMPro;
using UnityEngine;

public class FreePlaySongOption : MonoBehaviour {
    public TrackBundle associatedTrack;

    public int assumedDifficulty = 0;
    
    public TextMeshProUGUI diffText;
    public TextMeshProUGUI trackName;
    public TextMeshProUGUI artistName;

    public bool isFavorite;

    public void UpdateVisuals(TrackBundle tb) {
        associatedTrack = tb;
        
        Track t;
        if (associatedTrack.GetTrack(assumedDifficulty, out t)) {
            diffText.text = t.difficulty.ToString();
        }
        else {
            diffText.text = "-";
        }

        trackName.text = associatedTrack.trackName;
        artistName.text = associatedTrack.artistName;
    }

    public void UpdateAssumedDifficulty(int diff, TrackBundle newTrack = null) {
        assumedDifficulty = diff;
        if (newTrack) associatedTrack = newTrack;
        
        UpdateVisuals(associatedTrack);
    }

    public void Select() {
        SendMessageUpwards("SelectBundle", associatedTrack);
    }
}
