using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Graphics;

namespace CytusE.Game.Tests.Visual.UserInterface
{
    public class TestSceneTextButton : CytusETestScene
    {
        public TestSceneTextButton()
        {
            Add(new TextButton(true)
            {
                Action = Show,
                Text = "Positive",
                Width = 0.5f
            });
            Add(new TextButton(false)
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Action = Show,
                Text = "Negative",
                Width = 0.5f
            });
            Add(new TextButton(true)
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Width = 0.5f
            });
            Add(new TextButton(false)
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Width = 0.5f
            });
        }
    }
}


