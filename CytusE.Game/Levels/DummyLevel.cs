using System;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;

namespace CytusE.Game.Levels
{
    public class DummyLevel : Level
    {
        public override Track Track => new TrackVirtual(1000);

        public override Texture Background => null;

        public DummyLevel(ITrackStore tracks, LargeTextureStore textures)
            : base(string.Empty, tracks, textures)
        {
            LevelInfo = new LevelInfo
            {
                Artist = "please load a level!",
                Title = "no levels available!",
                Author = "Cytus E!",
                Bpm = 0,
                Difficulty = 0,
                Notes = Array.Empty<Note>(),
                Offset = 0,
                SpeedAdjustEvents = Array.Empty<SpeedAdjustEvent>()
            };
        }
    }
}
