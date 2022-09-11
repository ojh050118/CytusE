using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace CytusE.Game.Levels
{
    public class LevelStore : IResourceStore<Level>
    {
        public Storage Files { get; }

        private readonly ITrackStore tracks;
        private readonly LargeTextureStore textures;

        public LevelStore(Storage files, GameHost host, AudioManager audioManager)
        {
            Files = files;
            tracks = audioManager.GetTrackStore(new StorageBackedResourceStore(files));
            textures = new LargeTextureStore(host.Renderer, host.CreateTextureLoaderStore(new OnlineStore()));
            textures.AddTextureSource(host.CreateTextureLoaderStore(new StorageBackedResourceStore(files)));
        }

        public Level Get(string name)
        {
            if (Files.Exists(name))
                return new Level(Files.GetFullPath(name), tracks, textures);

            return null!;
        }

        public Task<Level> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            if (Files.Exists(name))
                return new Task<Level>(() => new Level(Files.GetFullPath(name), tracks, textures));

            return null;
        }

        public Stream GetStream(string name)
        {
            return Stream.Null;
        }

        public IEnumerable<string> GetAvailableResources()
        {
            List<string> levels = new List<string>();

            foreach (var dir in Files.GetDirectories(string.Empty))
            {
                var dirInfo = new DirectoryInfo(Files.GetFullPath(dir));

                foreach (var level in dirInfo.GetFiles($"*{Level.FILE_EXTENSION}"))
                {
                    try
                    {
                        using (StreamReader sr = level.OpenText())
                        {
                            JsonConvert.DeserializeObject<LevelInfo>(sr.ReadToEnd(), new JsonSerializerSettings
                            {
                                Formatting = Formatting.Indented
                            });
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (level.Directory != null)
                        levels.Add(Path.Combine(level.Directory.Name, level.Name));
                }
            }

            return levels;
        }

        public void Dispose()
        {
            tracks.Dispose();
            textures.Dispose();
        }

        public override string ToString() => $"Available level count: {GetAvailableResources().Count().ToString()}";
    }
}
