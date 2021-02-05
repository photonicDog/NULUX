using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Gamestate", menuName = "ScriptableObjects/Charting/Track", order = 1)]
public class Track : SerializedScriptableObject {

    [TitleGroup("Track Metadata")] public string trackName;
    [TitleGroup("Track Metadata")] public string releaseDate;
    [TitleGroup("Track Metadata")] public string genre;
    [TitleGroup("Track Metadata")] public float bpm;
    [TitleGroup("Track Metadata")] public int difficulty;
    [TitleGroup("Track Metadata")] public string albumPath;
    [TitleGroup("Track Metadata")] public string trackBGPath;

    [TitleGroup("Story Mode Metadata")] public bool isStoryModeTrack;
    [TitleGroup("Story Mode Metadata")] public WalkaroundNPCScenarioState songScenarioState;
    [TitleGroup("Story Mode Metadata")] public YarnProgram yp;

    [TitleGroup("Track Parameters")] public AudioClip Audio;
    [TitleGroup("Track Parameters")] public string PathToChart;
    [TitleGroup("Track Parameters")] public TrackLines Lines;
    [TitleGroup("Track Parameters")] public bool Failable;

    [TitleGroup("StoryModeParameters")] [ShowIf("isStoryModeTrack")]
    public Dictionary<string, bool> postSongFlagsBool;
    [TitleGroup("StoryModeParameters")] [ShowIf("isStoryModeTrack")]
    public Dictionary<string, int> postSongFlagsInt;
}
