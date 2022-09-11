using System;
using System.IO;
using Newtonsoft.Json;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;

namespace CytusE.Game.Levels
{
    public class Level : IEquatable<Level>
    {
        public const string FILE_EXTENSION = @".cytus";

        public LevelInfo LevelInfo { get; protected set; }

        public FileInfo LevelFile { get; }

        private readonly ITrackStore tracks;
        private readonly LargeTextureStore textures;

        public virtual Track Track
        {
            get
            {
                if (File.Exists(TrackPath))
                    return tracks.Get(TrackPath);

                return new TrackVirtual(1000);
            }
        }

        public virtual Texture Background
        {
            get
            {
                if (File.Exists(BackgroundPath))
                    return textures.Get(BackgroundPath);

                return null;
            }
        }

        public string TrackPath { get; private set; }
        public string BackgroundPath { get; private set; }

        public Level(string fileName, ITrackStore tracks, LargeTextureStore textures)
        {
            this.textures = textures;
            this.tracks = tracks;

            if (string.IsNullOrEmpty(fileName))
                return;

            LevelFile = new FileInfo(fileName);
            LevelInfo = parse(LevelFile);

            if (LevelInfo == null || LevelFile.DirectoryName == null)
                return;

            TrackPath = Path.Combine(LevelFile.DirectoryName, LevelInfo.SongFileName);
            BackgroundPath = Path.Combine(LevelFile.DirectoryName, LevelInfo.BackgroundFileName);
        }

        public void Refresh()
        {
            LevelFile.Refresh();
            LevelInfo = parse(LevelFile);

            if (LevelInfo == null || LevelFile.DirectoryName == null)
                return;

            TrackPath = Path.Combine(LevelFile.DirectoryName, LevelInfo.SongFileName);
            BackgroundPath = Path.Combine(LevelFile.DirectoryName, LevelInfo.BackgroundFileName);
        }

        private LevelInfo parse(FileInfo file)
        {
            LevelInfo info = null;

            using (StreamReader sr = file.OpenText())
            {
                try
                {
                    info = JsonConvert.DeserializeObject<LevelInfo>(sr.ReadToEnd());
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Failed to parse level {file.Name}.");
                }
            }

            return info;
        }

        /// <summary>
        /// 레벨 파일 이름을 보여줍니다.
        /// </summary>
        /// <returns>[Author] Artist - Song (Difficulty).cytus</returns>
        public override string ToString() => LevelInfo + FILE_EXTENSION;

        public bool Equals(Level level)
        {
            if (level == null)
                return false;

            return LevelInfo.Equals(level.LevelInfo) &&
                   TrackPath == level.TrackPath &&
                   BackgroundPath == level.BackgroundPath;
        }
    }
}
