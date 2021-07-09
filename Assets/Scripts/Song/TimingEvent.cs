using System;

[Serializable]
public class TimingEvent {
    public double time;
    public float bpm;

    public TimingEvent(double time, float bpm) {
        this.time = time;
        this.bpm = bpm;
    }
}
