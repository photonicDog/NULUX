using DeMetabolizerNovaEditor.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeMetabolizerNovaEditor.Types.Interfaces
{
    public interface INote
    {
        NoteTypes NoteType { get; set; }
        int StartBeat { get; set; }
        float Subdivision { get; set; }
        int Position { get; set; }
        float Duration { get; set; }
    }
}
