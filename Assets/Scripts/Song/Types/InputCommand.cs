namespace Song.Types {
    public class InputCommand {
        public double Time;
        public int Key;
        public PressType PressType;
    }

    public enum PressType {
        PRESS,
        RELEASE
    }
}