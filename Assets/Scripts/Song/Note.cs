using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Note {
    public double Start;
    public int NoteType;
    public bool Hit = false;
    public double Duration;
    public float Position;
    public int Index;

    public Note(double start, int n, float position) {
        this.Start = start;
        this.Position = position;
        this.NoteType = n;
        this.Duration = 0;
    }
    
    public Note(double start, int n, float position, double duration) {
        this.Start = start;
        this.Position = position;
        this.NoteType = n;
        this.Duration = duration;
    }
}