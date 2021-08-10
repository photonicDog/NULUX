using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Song.Types
{
    [Serializable]
    public class Chart
    {
        public List<List<Note>> Notes;
        public List<HitLine> Lines;
        public List<DialogueEvent> Dialogue;
        public List<TimingEvent> TimingEvents;
        public string AudioFile;

        public static Chart DeserialiseChartFile(string chartJSON)
        {
            return JsonConvert.DeserializeObject<Chart>(chartJSON);
        }
    }
}
