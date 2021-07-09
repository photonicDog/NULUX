using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using NAudio;
using NAudio.Midi;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class Conductor : MonoBehaviour {

    public static Conductor Instance;
    
    public AudioSource musicSource;
    [SerializeField]
    public double songTime;
    [SerializeField]
    public double startOffset; //the offset between the start of the audio file and the first beat
    [SerializeField]
    private double songStartPoint; //where in the audio file should we start playing from
    [SerializeField]
    private double leadIn; //how far ahead we schedule playback to give the audio system time to prepare
    [SerializeField]
    private double pausePoint; //stores the time of playback for when things get paused
    [SerializeField]
    private double startTimeAudio; //the dsptime when the music will start playing
    [SerializeField]
    private double startTimeReal; //the real time when the music will start playing

    public volatile bool isPlaying;

    private TimingEvent currentTiming;
    public double msPerBeat;
    public double nextBeat;

    private void Awake() {
        Instance = this;
    }

    public void StartMusic() {
        songStartPoint = 0;
        musicSource.timeSamples = 0;
        songTime = -10000;
        startTimeReal = Time.realtimeSinceStartupAsDouble + leadIn;
        startTimeAudio = AudioSettings.dspTime + leadIn;
        musicSource.PlayScheduled(startTimeAudio);
        isPlaying = true;
    }

    public void StartMusicFromTime(double time) {
        songStartPoint = time;
        musicSource.timeSamples = (int)(time * musicSource.clip.frequency);
        songTime = -10000;
        startTimeReal = Time.realtimeSinceStartupAsDouble + leadIn;
        startTimeAudio = AudioSettings.dspTime + leadIn;
        musicSource.PlayScheduled(startTimeAudio);
        isPlaying = true;
    }

    public void UpdateSongTime() {
        //update time
        songTime = ((songStartPoint + (Time.realtimeSinceStartupAsDouble - startTimeReal)) * 1000) + startOffset;
        //check timings
        
        //check beats
        if(songTime > nextBeat) {
            nextBeat += msPerBeat;
        }
    }

    public void UpdateCurrentTiming(TimingEvent timing) {
        Debug.Log("updating timing " + timing.bpm);
        currentTiming = timing;
        msPerBeat = 60 / currentTiming.bpm;
        msPerBeat *= 1000;
        nextBeat = songTime + (msPerBeat - ((songTime - currentTiming.time) % msPerBeat));
    }
}
