using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Newtonsoft.Json;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace CytusE.Game.Levels
{
    public class LevelManager
    {
        private readonly Storage export;
        private readonly LevelImporter levelImporter;

        public LevelStore LevelStore => levelImporter.Store;

        public LevelManager(Storage export, LevelStore store)
        {
            this.export = export;
            levelImporter = new LevelImporter(store);
        }

        public IEnumerable<Level> Import(FileInfo archive) => levelImporter.Import(archive);

        public IReadOnlyList<Level> GetAvailableLevels()
        {
            List<Level> levels = new List<Level>();

            foreach (var name in LevelStore.GetAvailableResources())
            {
                var level = LevelStore.Get(name);

                if (level.LevelInfo != null)
                    levels.Add(level);
            }

            return levels;
        }

        public Level CreateNew(LevelInfo info, string songFile, string backgroundFile)
        {
            var levelDirName = $"[{info.Author}] {info.Artist} - {info.Song}";
            var levelDir = levelImporter.Store.Files.GetStorageForDirectory(levelDirName).GetFullPath(string.Empty);
            var levelFileName = info + Level.FILE_EXTENSION;

            try
            {
                if (File.Exists(songFile))
                {
                    info.SongFileName = Path.GetFileName(songFile);
                    File.Copy(songFile, Path.Combine(levelDir, info.SongFileName));
                }

                if (File.Exists(backgroundFile))
                {
                    info.BackgroundFileName = Path.GetFileName(backgroundFile);
                    File.Copy(backgroundFile, Path.Combine(levelDir, info.BackgroundFileName));
                }

                var cytus = JsonConvert.SerializeObject(info, Formatting.Indented);

                using (StreamWriter sw = File.CreateText(Path.Combine(levelDir, levelFileName)))
                    sw.WriteLine(cytus);
            }
            catch (Exception e)
            {
                Logger.Error(e, "An error occurred while creating a new level.");
            }

            return levelImporter.Store.Get(Path.Combine(levelDirName, levelFileName));
        }

        public Level CreateNew(LevelInfo info) => CreateNew(info, info.SongFileName, info.BackgroundFileName);

        public string Export(Level level)
        {
            if (level.LevelFile.Directory == null)
                return string.Empty;

            var temp = export.GetStorageForDirectory(level.LevelInfo.GetName());

            foreach (var file in level.LevelFile.Directory.GetFiles())
                File.Copy(file.FullName, Path.Combine(temp.GetFullPath(string.Empty), file.Name), true);

            var archive = Path.Combine(export.GetFullPath(string.Empty), $"{level.LevelInfo.GetName()}.cytusz");

            if (export.Exists($"{level.LevelInfo.GetName()}.cytusz"))
                export.Delete($"{level.LevelInfo.GetName()}.cytusz");

            ZipFile.CreateFromDirectory(temp.GetFullPath(string.Empty), archive);
            temp.DeleteDirectory(string.Empty);

            return archive;
        }

        public async Task<Level> Save(Level level)
        {
            using (StreamWriter sw = level.LevelFile.CreateText())
            {
                try
                {
                    var cytus = JsonConvert.SerializeObject(level.LevelInfo, Formatting.Indented);

                    await sw.WriteLineAsync(cytus);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"An error occurred while saving level {level}");
                }
            }

            level.Refresh();
            return level;
        }

        public void Delete(Level level)
        {
            try
            {
                level.LevelFile.Directory?.Delete(true);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"An error occurred while deleting level directory {level.LevelFile.Directory?.Name}");
            }
        }
    }
}
