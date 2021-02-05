using DeMetabolizerNovaEditor.Types.Enums;
using DeMetabolizerNovaEditor.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeMetabolizerNovaEditor.Types.Implementations
{
    public class Note : INote
    {
        public Note(NoteTypes noteType, int beat, float subdivison, int position, float duration)
        {
            NoteType = noteType;
            StartBeat = beat;
            Subdivision = subdivison;
            Position = position;
            Duration = duration;
        }

        public NoteTypes NoteType {
            get => NoteType;
            set => NoteType = value;
        }

        public int StartBeat {
            get => StartBeat;
            set => StartBeat = value;
        }

        //note: this property is more like a decimal to the beat than a marker of subdivison, e.g. it can be 0.375, as in 3/8
        //this is because we don't need to mark the base subdivision, like 1/8, because that is global at any given time
        public float Subdivision {
            get => Subdivision;
            set => Subdivision = value;
        }

        public int Position {
            get => Position;
            set => Position = value;
        }

        public float Duration
        {
            get => Duration;
            set => Duration = value;
        }

        public void SnapSubdivision(float newSubdivision, bool toUpper)
        {
            //if subdivision isn't fine where it is (e.g. if a new subdivision is 0.125 (1/8), and the existing is 0.25 (1/4),
            //then don't change it). if it isn't, send it to nearest new subdivision based on toUpper
            if (Subdivision % newSubdivision != 0) 
            {
                var newSubdivisionOffset = Subdivision / newSubdivision;
                Subdivision = toUpper ? newSubdivisionOffset + 1 : newSubdivisionOffset;
            }
        }
    }
}
