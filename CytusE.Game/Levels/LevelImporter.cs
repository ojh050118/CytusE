using System.Collections.Generic;
using System.IO;
using System.Linq;
using CytusE.Game.IO.Archives;
using SharpCompress.Archives;

namespace CytusE.Game.Levels
{
    public class LevelImporter
    {
        public LevelStore Store { get; }

        public LevelImporter(LevelStore store)
        {
            Store = store;
        }

        public IEnumerable<Level> Import(FileInfo archive)
        {
            var levelDirName = Path.GetFileNameWithoutExtension(archive.Name);
            // 레벨 디렉터리를 압축파일 이름으로 생성합니다.
            var levelStorage = Store.Files.GetStorageForDirectory(levelDirName);
            List<Level> levels = new List<Level>();

            var di = new DirectoryInfo(levelStorage.GetFullPath(string.Empty));

            // 만약 이미 같은 이름의 디렉터리가 존재하면 모든 파일 삭제합니다.
            foreach (var file in di.GetFiles())
                file.Delete();

            // 압축파일을 레벨 디렉터리에 압축해제합니다.
            using (var reader = new ZipArchiveReader(archive.Open(FileMode.Open, FileAccess.Read), archive.Name))
                reader.Archive.WriteToDirectory(levelStorage.GetFullPath(string.Empty));

            // 레벨 디렉터리 안에 있는 레벨 파일들을 가져옵니다.
            foreach (var file in di.GetFiles($"*{Level.FILE_EXTENSION}"))
                levels.Add(Store.Get(Path.Combine(levelDirName, file.Name)));

            if (levels.Count == 0)
                return null;

            var level = levels.First();
            var levelName = $"[{level.LevelInfo.Author}] {level.LevelInfo.Artist} - {level.LevelInfo.Song}";

            // 레벨이 저장된 폴더의 이름을 변경합니다.
            if (levelStorage.GetFullPath(string.Empty) != Store.Files.GetStorageForDirectory(levelName).GetFullPath(string.Empty))
                levelStorage.Move(levelStorage.GetFullPath(string.Empty), Store.Files.GetStorageForDirectory(levelName).GetFullPath(string.Empty));

            foreach (var lvl in levels)
                lvl.Refresh();

            return levels;
        }
    }
}
