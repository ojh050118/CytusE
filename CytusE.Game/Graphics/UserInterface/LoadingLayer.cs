using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Graphics.UserInterface
{
    public class LoadingLayer : LoadingSpinner
    {
        private readonly Box dim;

        public override bool HandleNonPositionalInput => false;

        public LoadingLayer()
        {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            AddInternal(dim = new Box
            {
                Depth = float.MaxValue,
                Colour = Color4.Black,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0
            });
        }

        protected override void PopIn()
        {
            dim?.FadeTo(0.5f, TRANSITION_DURATION * 2, Easing.OutQuint);
            base.PopIn();
        }

        protected override void PopOut()
        {
            dim?.FadeOut(TRANSITION_DURATION, Easing.OutQuint);
            base.PopOut();
        }

        protected override void Update()
        {
            base.Update();

            var diff = DrawWidth - DrawHeight;
            var avg = (DrawWidth + DrawHeight) / 2;
            var diffRatio = diff / avg;

            MainContents.Scale = new Vector2(Math.Clamp((1 - Math.Abs(diffRatio) * 0.25f), 0.3f, 1));
        }
    }
}
