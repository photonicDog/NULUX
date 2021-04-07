using System.Collections;
using System.Collections.Generic;
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
            trackName.text = t.trackName;
            artistName.text = t.artistName;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public void UpdateAssumedDifficulty(int diff) {
        assumedDifficulty = diff;
        
        UpdateVisuals(associatedTrack);
    }

    public void Select() {
        SendMessageUpwards("SelectBundle", associatedTrack);
    }
}
