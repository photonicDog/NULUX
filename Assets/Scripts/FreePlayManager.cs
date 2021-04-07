// FreePlayManager
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
    public TextMeshProUGUI bpm;
    public TextMeshProUGUI release;
    public List<Button> difficultySelector;

    private void Awake()
    {
        foreach (TrackBundle tb in songList) {
            tb.UpdateDifficulties();
            GameObject sl = Instantiate(songOptionPrefab, songListObject.transform, false);
            sl.GetComponent<FreePlaySongOption>().UpdateVisuals(tb);
            sl.GetComponent<FreePlaySongOption>().UpdateAssumedDifficulty(assumedDifficulty);
        }
    }

    public void SelectBundle(TrackBundle t) {
        selectedBundle = t;
        if (selectedBundle.GetTrack(assumedDifficulty, out selectedTrack)) UpdateUI();
    }

    public void LaunchSong(Track t) {
        selectedTrack = t;
        Instantiate(songCarrier).GetComponent<SongLaunchCarrier>().Execute(selectedTrack);
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(2));
    }

    private void UpdateUI() {
        title.text = selectedTrack.trackName;
        artist.text = selectedTrack.artistName;
        bpm.text = selectedTrack.bpm.ToString();
        release.text = selectedTrack.releaseDate;

        for (int i = 0; i < difficultySelector.Count; i++) {
            difficultySelector[i].interactable = selectedBundle.availableDifficulties[i];
        }
    }

    public void SetDifficulty(int i) {
        assumedDifficulty = i;
    }

    public void UpdateList() {
        
    }
}
