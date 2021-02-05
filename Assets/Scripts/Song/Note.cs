using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Note {
    public float Start;
    public NoteType NoteType;
    public bool Hit = false;
    public float Duration;
    public float Position;
    public int Index;

    public Note(float start, int n, float position) {
        this.Start = start;
        this.Position = position;
        this.NoteType = (NoteType)n;
        this.Duration = 0;
    }
    
    public Note(float start, int n, float position, float duration) {
        this.Start = start;
        this.Position = position;
        this.NoteType = (NoteType)n;
        this.Duration = duration;
    }
}

public enum NoteType {
    L1 = 0, 
    L2 = 1, 
    L3 = 2, 
    L4 = 3,
}