using System.Collections.Generic;
using Assets.Scripts.Song.Enums;
using Assets.Scripts.Song.Extensions;
using Song.Types;
using UnityEngine;

namespace Song {
    public class NoteManager : MonoBehaviour {

        private List<Note> activeNotes;

        private List<List<Note>> notes;
    
        private Note[] holds;
        private int[] noteListIndices;

        private JudgementWindow jw;

        private static NoteManager _instance;
        public static NoteManager Instance { get { return _instance; } }
        void Awake() {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            
            holds = new Note[7];
            notes = new List<List<Note>>();
            noteListIndices = new int[4];
            jw = SongGameplayManager.Instance.settings.judgementWindow;
        }
        public void BuildNoteLists() {
            //ChartDataManager pull goes here
        }

        public void OnUpdate(Queue<InputCommand> commandQueue) {
            var time = Conductor.Instance.GetSongTime();
            foreach (InputCommand cmd in commandQueue) {
                CheckInput(cmd, time);  
            }
            CheckCurrentNotes(time);
            CheckCurrentHolds(time);
        }

        //Looks at the notes at the current indices for every note list and checks if any of them have been missed.
        //Updates note list indices accordingly, sends miss events for any missed notes
        private void CheckCurrentNotes(double time) {
            int missThreshold = jw.offWindow;
            for (int i = 0; i < 4; i++) {
                Note currentNote = notes[i][noteListIndices[i]];
                if (currentNote.Start < time - missThreshold) {
                    SendHit(currentNote, false,-missThreshold-1);
                    noteListIndices[i]++;
                }
            }
        }
    
        //Looks at any current holds and checks if there is an ongoing corresponding held input from SongInputManager.
        //Clears the hold from hold list if dropped and creates and sends a miss event

        private void CheckCurrentHolds(double time) {
            int missThreshold = jw.offWindow;
            for(int i = 0; i < 7; i++) {
                if (holds[i] != null) {
                    if (holds[i].Start + holds[i].Duration < time - missThreshold) {
                        SendHit(holds[i], true,-missThreshold-1);
                        holds[i] = null;
                    }
                }
            }
        }

        /*
*      If release event, check hold list.
*          If index is true, set false, do hit logic on note release time.
    ELSE, IF PRESS:
    Checks command’s associated note list at current list index and compares against time. 
        If early and outside of timing window, disregard.
        If inside timing window, do hit logic and iterate list index.
            Additionally, if hold, set bool in hold list to true.
*/
        private void CheckInput(InputCommand command, double time) {
            // Because there are duplicate keys (SDF JKL) for 3 tracks, simplify for array reasons.
            // Use command.Key for hold stuff.
            
            NoteType boundedKey = KeyToNoteType.FromKeyType(command.Key);
            int keyValue = (int)command.Key;

            if (command.PressType == PressType.RELEASE) {
                if (holds[keyValue] != null) {
                    double timeDifference = CalculateTimingDifference(holds[keyValue].Start + holds[keyValue].Duration, command.Time);
                    SendHit(holds[keyValue], true, timeDifference);
                    holds[keyValue] = null;
                }
            }
            else {
                Note currentNote = notes[(int)boundedKey][noteListIndices[(int)boundedKey]];

                if (currentNote.Start > time + jw.closeWindow) {
                    return;
                }
                else {
                    if (currentNote.Duration > 0) {
                        holds[keyValue] = currentNote;
                    }
                    double timeDifference = CalculateTimingDifference(currentNote.Start, command.Time);
                    SendHit(currentNote, false, timeDifference);
                }
            }
        }

        private double CalculateTimingDifference(double note, double input) {
            return input - note;
        }
    
        //Creates and sends HitData to SongGameplayManager. Used for misses as well.
        private void SendHit(Note note, bool release, double timeDifference) {
            HitEvent ev = new HitEvent(note, release, timeDifference);
            
            SongGameplayManager.Instance.AddToHitEventQueue(ev);
        }
    }
}
