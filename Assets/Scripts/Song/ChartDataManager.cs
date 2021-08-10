using Assets.Scripts.Song.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Song
{
    public class ChartDataManager : MonoBehaviour
    {

        private static ChartDataManager _instance;
        public static ChartDataManager Instance { get { return _instance; } }

        public Chart _chart;

        private List<List<Note>> _notes;
        private List<HitLine> _lines;
        private List<DialogueEvent> _dialogue;
        private List<TimingEvent> _timingEvents;

        // Singleton instancing
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            _notes = new List<List<Note>>();
            _lines = new List<HitLine>();
            _dialogue = new List<DialogueEvent>();
            _timingEvents = new List<TimingEvent>();
        }

        // Parses chart json files and saves values as properties
        public void ParseChartJson(string ChartJson)
        {
            _chart = Chart.DeserialiseChartFile(ChartJson);
            _notes = _chart.Notes;
            _lines = _chart.Lines;
            _dialogue = _chart.Dialogue;
            _timingEvents = _chart.TimingEvents;
        }

        // Returns chart notes
        public List<List<Note>> GetNotes()
        {
            return _notes;
        }

        // Returns chart lines
        public List<HitLine> GetLines()
        {
            return _lines;
        }

        // Returns any dialogue events
        public List<DialogueEvent> GetDialogue()
        {
            return _dialogue;
        }

        public List<TimingEvent> GetTimings()
        {
            return _timingEvents;
        }
    }
}