namespace Song.Types
{
    public class DialogueEvent
    {
        public double Time;
        public string YarnNode;
        
        public DialogueEvent(double time)
        {
            Time = time;
            YarnNode = "";
        }

        public DialogueEvent(double time, string yarnNode)
        {
            Time = time;
            YarnNode = yarnNode;
        }
    }
}
