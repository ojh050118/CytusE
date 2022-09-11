using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using osuTK.Input;

namespace CytusE.Game.Graphics.Containers
{
    public abstract class CytusEFocusedOverlayContainer : FocusedOverlayContainer
    {
        protected override bool BlockNonPositionalInput => State.Value == Visibility.Visible;

        protected override bool StartHidden => true;

        protected virtual float Dim => 0.75f;

        protected override Container<Drawable> Content => content;

        private readonly Container content = new Container { RelativeSizeAxes = Axes.Both };

        private readonly Box bg;

        protected CytusEFocusedOverlayContainer()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                bg = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                content
            };
        }

        protected override void PopIn()
        {
            bg.FadeTo(Dim, 200, Easing.Out);
        }

        protected override void PopOut()
        {
            bg.FadeOut(200, Easing.Out);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        Hide();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }
    }
}
