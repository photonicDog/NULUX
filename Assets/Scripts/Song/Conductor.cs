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
    public float bpm;

    private Stopwatch stopwatch;
    
    public float songPosition;
    public float offset;

    private bool priming;
    private bool primed;
    private Stopwatch primedWatch;
    private TimeSpan primedTimer;
    private float primedTime;

    public float beatS;
    public float halfS;
    public float quarterS;
    public float eighthS;
    public float sixteenthS;

    public bool playing;
    private Dictionary<int, char> midNoteToKey;
    
    public AudioSource musicSource;
    
    private static Conductor _instance;
    public static Conductor Instance {
        get { return _instance; }
    }
    private void Awake() {
        if (Instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
        
        stopwatch = new Stopwatch();
        primedWatch = new Stopwatch();
        if (!Stopwatch.IsHighResolution) {
            Debug.Log("Not using a high resolution timer!");
        }

    }

    // Start is called before the first frame update
    void Start() {
        beatS = (60f / bpm);
        halfS = beatS / 2;
        quarterS = halfS / 2;
        eighthS = quarterS / 2;
        sixteenthS = eighthS / 2;
    }

    public void SetSong(AudioClip au) {
        musicSource.clip = au;
    }

    // Update is called once per frame
    void Update() {
        if (playing) UpdateTiming();
    }

    public void SetSongOffset(float offset) {
        this.offset = offset;
    }

    public void SetBPM(float bpm) {
        this.bpm = bpm;
    }

    public void PrimeSong(float time) {
        playing = true;
        primed = false;
        primedTime = time;
        primedWatch.Start();
        
        StartCoroutine(SongDelay(time));
    }

    IEnumerator SongDelay(float time) {
        yield return new WaitForSeconds(time);
        primedTimer = primedWatch.Elapsed;
        primedWatch.Stop();
        primed = true;
        
        StartSong();
    }

    public void StartSong() {
        stopwatch.Reset();
        stopwatch.Start();

        if (primed) stopwatch.Elapsed.Add(primedTimer);
        musicSource.Play(0);
        
        playing = true;
    }

    public void StopSong() {
        stopwatch.Stop();
        musicSource.Stop();
        
        playing = false;
    }

    public void ResetSong(float time) {
        StopSong();
        PrimeSong(time);
    }

    public void UpdateTiming() {
        songPosition = GetSongTime();
    }

    public float GetSongTime() {
        return (GetSongTimeMS() / 1000f) * (bpm / 60f);
        
        throw new Exception("Stopwatch is not running!");
    }

    public long GetSongTimeMS() {
        if (stopwatch.IsRunning || primedWatch.IsRunning) {
            if (primedWatch.IsRunning) return ((long) -(primedTime * 1000)) + primedWatch.ElapsedMilliseconds + Mathf.RoundToInt(offset);
            return stopwatch.ElapsedMilliseconds + Mathf.RoundToInt(offset);
        }
        
        throw new Exception("Stopwatch is not running!");
    }
}
