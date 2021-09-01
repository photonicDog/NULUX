using Assets.Scripts.Song.Enums;

namespace Song.Types {
    public class InputCommand {
        public double Time;
        public KeyType Key;
        public PressType PressType;

        public InputCommand(double time, KeyType key, PressType pressType)
        {
            Time = time;
            Key = key;
            PressType = pressType;
        }
    }
}