// FreePlayManager

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FreePlayManager : MonoBehaviour
{
    public Track selectedTrack;
    public TrackBundle selectedBundle;

    public int assumedDifficulty = 2;

    public List<TrackBundle> songList;
    public GameObject songListObject;

    public GameObject songOptionPrefab;

    public GameObject songCarrier;



    [Header("UI")] public TextMeshProUGUI title;
    public TextMeshProUGUI artist;
    public TextMeshProUGUI genre;
    public TextMeshProUGUI bpm;
    public TextMeshProUGUI release;
    public List<Button> difficultySelector;

    private void Awake()
    {
        foreach (TrackBundle tb in songList) {
            tb.UpdateDifficulties();
            GameObject sl = Instantiate(songOptionPrefab, songListObject.transform, false);
            sl.GetComponent<FreePlaySongOption>().UpdateAssumedDifficulty(assumedDifficulty, tb);
        }
        
        SelectBundle(songList[0]);
    }

    public void SelectBundle(TrackBundle t) {
        selectedBundle = t;
        if (selectedBundle.GetTrack(assumedDifficulty, out selectedTrack)) {
            UpdateUI();
        }
        else {
            if (selectedBundle.GetGeneric(out selectedTrack))
            UpdateUI();
        }
    }

    public void LaunchSong() {
        if (!selectedTrack) return;
        Instantiate(songCarrier).GetComponent<SongLaunchCarrierDeprecated>().Execute(selectedTrack);
        GetComponent<MusicSelectMenu>().LaunchFreePlay();
    }

    private void UpdateUI() {
        title.text = selectedTrack.trackName;
        genre.text = selectedTrack.genre;
        artist.text = selectedTrack.artistName;
        bpm.text = selectedTrack.bpm.ToString();
        release.text = selectedTrack.releaseDate;

        for (int i = 0; i < difficultySelector.Count; i++) {
            difficultySelector[i].interactable = selectedBundle.availableDifficulties[i];
            Track t;
            if (selectedBundle.GetTrack(i, out t)) {
                difficultySelector[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = t.difficulty.ToString();
            }
            else {
                difficultySelector[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = "X";
            }
        }

        if (selectedBundle.GetTrack(assumedDifficulty, out selectedTrack)) {
            difficultySelector[assumedDifficulty].Select();
        }
        else {
            for (int i = 0; i < difficultySelector.Count; i++) {
                if (selectedBundle.GetTrack(i, out selectedTrack)) {
                    difficultySelector[i].Select();
                    break;
                }
            }

            if (!selectedTrack) {
                throw new Exception("Something went wrong with selecting the default track.");
            }
        }

    }

    public void SetDifficulty(int i) {
        assumedDifficulty = i;
    }

    public void UpdateList() {
        
    }

}
