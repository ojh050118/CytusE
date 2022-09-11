using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CytusE.Game.Levels;
using NUnit.Framework;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace CytusE.Game.Tests.Levels
{
    [TestFixture]
    public class LevelManagerTest
    {
        private readonly LevelInfo levelInfo = new LevelInfo
        {
            Author = "Cytus E!",
            Title = "Bullet waiting for me (James Landino Remix)",
            Song = "Bullet waiting for me (James Landino Remix)",
            Artist = "Cytus 2",
            BackgroundFileName = "bg.png",
            Bpm = 123,
            Difficulty = 5,
            Notes = Array.Empty<Note>(),
            Offset = 0,
            SongFileName = "Bullet waiting for me (James Landino Remix).mp3",
            SpeedAdjustEvents = Array.Empty<SpeedAdjustEvent>()
        };

        [Test]
        public void TestLevelCreate()
        {
            using (HeadlessGameHost host = new TestRunHeadlessGameHost())
            {
                try
                {
                    var cytuse = loadCytusEIntoHost(host);

                    var level = cytuse.LevelManager.CreateNew(levelInfo);
                    Debug.Assert(level != null);
                }
                finally
                {
                    host.Exit();
                }
            }
        }

        [Test]
        public async Task TestLevelSave()
        {
            using (HeadlessGameHost host = new TestRunHeadlessGameHost())
            {
                try
                {
                    var cytuse = loadCytusEIntoHost(host);

                    var level = cytuse.LevelManager.CreateNew(levelInfo);
                    Debug.Assert(level != null);

                    level.LevelInfo.Difficulty = 7;
                    level.LevelInfo.BackgroundFileName = "new-bg.png";
                    level.LevelInfo.Title = "Edited - Bullet waiting for me";

                    await cytuse.LevelManager.Save(level).ConfigureAwait(false);
                }
                finally
                {
                    host.Exit();
                }
            }
        }

        [Test]
        public void TestLevelExport()
        {
            using (HeadlessGameHost host = new TestRunHeadlessGameHost())
            {
                try
                {
                    var cytuse = loadCytusEIntoHost(host);

                    var level = cytuse.LevelManager.CreateNew(levelInfo);
                    Debug.Assert(level != null);

                    cytuse.LevelManager.Export(level);
                }
                finally
                {
                    host.Exit();
                }
            }
        }

        [Test]
        public void TestLevelArchiveImport()
        {
            using (HeadlessGameHost host = new TestRunHeadlessGameHost())
            {
                try
                {
                    var cytuse = loadCytusEIntoHost(host);

                    var level = cytuse.LevelManager.CreateNew(levelInfo);
                    Debug.Assert(level != null);

                    var archivePath = cytuse.LevelManager.Export(level);
                    cytuse.LevelManager.Import(new FileInfo(archivePath));
                }
                finally
                {
                    host.Exit();
                }
            }
        }

        [Test]
        public void TestLevelDelete()
        {
            using (HeadlessGameHost host = new TestRunHeadlessGameHost())
            {
                try
                {
                    var cytuse = loadCytusEIntoHost(host);

                    var level = cytuse.LevelManager.CreateNew(levelInfo);
                    Debug.Assert(level != null);

                    cytuse.LevelManager.Delete(level);
                }
                finally
                {
                    host.Exit();
                }
            }
        }

        #region Test host creation

        private TestCytusEGameBase loadCytusEIntoHost(GameHost host)
        {
            var cytuse = new TestCytusEGameBase();
            Task.Factory.StartNew(() => host.Run(cytuse), TaskCreationOptions.LongRunning)
                .ContinueWith(t => Assert.Fail($"Host threw exception {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted);

            waitForOrAssert(() => cytuse.IsLoaded, @"Cytus E! failed to start in a reasonable amount of time");

            bool ready = false;
            host.UpdateThread.Scheduler.Add(() => host.UpdateThread.Scheduler.Add(() => ready = true));

            waitForOrAssert(() => ready, @"Cytus E! failed to start in a reasonable amount of time");

            return cytuse;
        }

        private void waitForOrAssert(Func<bool> result, string failureMessage, int timeout = 60000)
        {
            Task task = Task.Run(() =>
            {
                while (!result()) Thread.Sleep(200);
            });

            Assert.IsTrue(task.Wait(timeout), failureMessage);
        }

        public class TestCytusEGameBase : CytusEGameBase
        {
            public new LevelStore LevelStore => base.LevelStore;

            public new LevelManager LevelManager => base.LevelManager;
        }

        #endregion
    }
}


