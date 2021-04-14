using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "TrackBundle", menuName = "ScriptableObjects/Charting/TrackBundle", order = 1)]
public class TrackBundle : SerializedScriptableObject {
    public List<Track> associatedTracks;
    public List<bool> availableDifficulties;

    public bool storyMode;
    
    [TitleGroup("Track Metadata")] public string trackName;
    [TitleGroup("Track Metadata")] public string artistName;
    [TitleGroup("Track Metadata")] public string releaseDate;
    [TitleGroup("Track Metadata")] public string genre;
    [TitleGroup("Track Metadata")] public string albumPath;
    [TitleGroup("Track Metadata")] public string trackBGPath;

    public void PropagateBlankParameters(int index) {
        Track asscTrack = associatedTracks[index];
        if (asscTrack.trackName == null) asscTrack.trackName = trackName;
        if (asscTrack.artistName == null) asscTrack.artistName = artistName;
        if (asscTrack.releaseDate == null) asscTrack.releaseDate = releaseDate;
        if (asscTrack.genre == null) asscTrack.genre = genre;
        if (asscTrack.albumPath == null) asscTrack.albumPath = albumPath;
        if (asscTrack.trackBGPath == null) asscTrack.trackBGPath = trackBGPath;

        asscTrack.isStoryModeTrack = storyMode;
    }

    public bool GetTrack(int index, out Track t) {
        if (associatedTracks[index]) {
            t = associatedTracks[index];
            return true;
        }

        t = null;
        return false;
    }

    public bool GetGeneric(out Track t) {
        t = associatedTracks.Find(a => a != null);
        return t != null;
    }

    public void UpdateDifficulties() {
        for (int i = 0; i < 5; i++) {
            Track t;
            availableDifficulties[i] = GetTrack(i, out t);
        }
    }
}
