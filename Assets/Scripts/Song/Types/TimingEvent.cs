using System;

namespace Song.Types {
    [Serializable]
    public class TimingEvent {
        public double Time;
        public float BPM;

        public TimingEvent(double time, float bpm) {
            Time = time;
            BPM = bpm;
        }
    }
}
