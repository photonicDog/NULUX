namespace Song.Types {
    public class HitEvent {
        public Note Note;
        public double Offset;
        public bool Release;

        public HitEvent(Note note, bool release, double offset) {
            Note = note;
            Offset = offset;
            Release = release;
        }
    }
}
