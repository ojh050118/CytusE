using osu.Framework.Testing;

namespace CytusE.Game.Tests.Visual
{
    public class CytusETestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new CytusETestSceneTestRunner();

        private class CytusETestSceneTestRunner : CytusEGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
