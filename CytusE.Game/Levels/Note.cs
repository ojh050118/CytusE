using System;
using osu.Framework.Utils;
using osuTK;

namespace CytusE.Game.Levels
{
    public class Note : IEquatable<Note>
    {
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public Vector2 Position { get; set; }
        public Vector2[] Points { get; set; }
        public NoteType NoteType { get; set; }

        public bool Equals(Note note)
        {
            if (note == null)
                return false;

            return Precision.AlmostEquals(StartTime, note.StartTime) &&
                   Precision.AlmostEquals(EndTime, note.EndTime) &&
                   Precision.AlmostEquals(Position, note.Position) &&
                   Points.Length == note.Points.Length &&
                   NoteType == note.NoteType;
        }
    }
}
