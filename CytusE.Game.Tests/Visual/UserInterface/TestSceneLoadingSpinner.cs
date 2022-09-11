using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Graphics;

namespace CytusE.Game.Tests.Visual.UserInterface
{
    public class TestSceneLoadingSpinner : CytusETestScene
    {
        public TestSceneLoadingSpinner()
        {
            LoadingSpinner spinner;

            Add(spinner = new LoadingSpinner
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
            AddStep("Toggle visibility", spinner.ToggleVisibility);
        }
    }
}
