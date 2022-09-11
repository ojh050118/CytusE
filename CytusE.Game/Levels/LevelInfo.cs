using System;
using osu.Framework.Utils;

namespace CytusE.Game.Levels
{
    public class LevelInfo : IEquatable<LevelInfo>
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Song { get; set; }
        public string SongFileName { get; set; }
        public string BackgroundFileName { get; set; }
        public string Artist { get; set; }
        public int Difficulty { get; set; }
        public float Bpm { get; set; }
        public float Offset { get; set; }

        public Note[] Notes { get; set; }
        public SpeedAdjustEvent[] SpeedAdjustEvents { get; set; }

        public string GetName() => $"[{Author}] {Artist} - {Song}";

        /// <summary>
        /// 레벨의 기본 정보를 보여줍니다.
        /// </summary>
        /// <returns>[Author] Artist - Song (Difficulty)</returns>
        public override string ToString() => $"[{Author}] {Artist} - {Song} ({Difficulty.ToString()})";

        public bool Equals(LevelInfo info)
        {
            if (info == null)
                return false;

            return Author == info.Author &&
                   Title == info.Title &&
                   Song == info.Song &&
                   SongFileName == info.SongFileName &&
                   BackgroundFileName == info.BackgroundFileName &&
                   Artist == info.Artist &&
                   Difficulty == info.Difficulty &&
                   Precision.AlmostEquals(Bpm, info.Bpm) &&
                   Precision.AlmostEquals(Offset, info.Offset) &&
                   Notes.Length == info.Notes.Length &&
                   SpeedAdjustEvents.Length == info.SpeedAdjustEvents.Length;
        }
    }

    public struct SpeedAdjustEvent : IEquatable<SpeedAdjustEvent>
    {
        public float Time { get; set; }
        public float Bpm { get; set; }

        public bool Equals(SpeedAdjustEvent e)
        {
            return Precision.AlmostEquals(Time, e.Time) &&
                   Precision.AlmostEquals(Bpm, e.Bpm);
        }
    }
}
