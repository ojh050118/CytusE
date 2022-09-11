using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace CytusE.Game.Tests.Visual.UserInterface
{
    public class TestSceneLoadingLayer : CytusETestScene
    {
        public TestSceneLoadingLayer()
        {
            LoadingLayer spinner;
            Container container;

            Add(new Box()
            {
                Colour = Color4.Gray,
                RelativeSizeAxes = Axes.Both
            });
            Add(container = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Child = spinner = new LoadingLayer()
            });
            AddStep("Toggle visibility", spinner.ToggleVisibility);
            AddSliderStep("Width", 0.1f, 1, 1f, v => container.Width = v);
            AddSliderStep("Height", 0.1f, 1, 1f, v => container.Height = v);
        }
    }
}
