using Assets.Scripts.Song.Enums;
using System;

namespace Assets.Scripts.Song.Extensions
{
    public static class KeyToNoteType 
    {
        public static NoteType FromKeyType(this KeyType keyType)
        {
            switch (keyType)
            {
                case KeyType.L1:
                case KeyType.R1:
                    return NoteType.Blue;

                case KeyType.L2:
                case KeyType.R2:
                    return NoteType.Pink;

                case KeyType.L3:
                case KeyType.R3:
                    return NoteType.Green;

                case KeyType.Bar:
                    return NoteType.Bar;

                default:
                    throw new Exception($"Tried to convert a KeyType value not implemented into the mapper: {keyType}");
            }
        }
    }
}
