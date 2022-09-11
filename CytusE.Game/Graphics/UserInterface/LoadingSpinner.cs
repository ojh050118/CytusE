using System;
using System.Collections.Generic;
using CytusE.Game.Graphics.Containers;
using CytusE.Game.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Graphics.UserInterface
{
    public class LoadingSpinner : VisibilityContainer
    {
        public const float TRANSITION_DURATION = 250;
        public const int FLASHING_CIRCLE_COUNT = 8;

        protected override bool StartHidden => true;

        protected Container MainContents;
        private readonly Container<LoadingCircle> flash;

        public LoadingSpinner()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(270);
            Child = MainContents = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new LoadingText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    flash = new Container<LoadingCircle>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Children = createCircles(210)
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            rotate();
        }

        private LoadingCircle[] createCircles(float width)
        {
            List<LoadingCircle> circles = new List<LoadingCircle>();
            const float rotation_per_circle = 360f / FLASHING_CIRCLE_COUNT;

            for (int i = 0; i < FLASHING_CIRCLE_COUNT; i++)
            {
                var rotation = -1 * rotation_per_circle * i + 180;
                float posX = width / 2 * MathF.Sin(MathHelper.DegreesToRadians(rotation));
                float posY = width / 2 * MathF.Cos(MathHelper.DegreesToRadians(rotation));

                circles.Add(new LoadingCircle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(posX, posY),
                    Size = new Vector2(17),
                    Alpha = 0.5f
                });
            }

            return circles.ToArray();
        }

        private void rotate()
        {
            const int delay = 80;

            var startOffset = 0;
            var revolutionPause = delay * flash.Count - TRANSITION_DURATION;

            for (int i = 0; i < flash.Count; i++)
            {
                var circle = flash[i];
                startOffset += delay;
                circle.Fill.Colour = Color4.Transparent;

                circle.ClearTransforms();
                circle.Fill.ClearTransforms();
                circle.Delay(startOffset).Then().Loop(revolutionPause, d => d.FadeTo(1).FadeTo(0.5f, TRANSITION_DURATION));
                circle.Fill.Delay(startOffset).Then().Loop(revolutionPause, d => d.FadeColour(Color4.White).FadeColour(Color4.Transparent, TRANSITION_DURATION));
            }
        }

        protected override void PopIn()
        {
            if (Alpha < 0.5f)
                rotate();

            this.FadeIn(TRANSITION_DURATION * 2, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);
        }

        private class LoadingCircle : Container
        {
            public readonly Box Fill;

            public LoadingCircle()
            {
                InternalChild = new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    BorderColour = Color4.White,
                    BorderThickness = 2,
                    Masking = true,
                    Child = Fill = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Transparent,
                        AlwaysPresent = true
                    }
                };
            }
        }

        private class LoadingText : CornerDotContainer
        {
            public LoadingText()
            {
                ShowLine = false;
                RelativeSizeAxes = Axes.None;
                AutoSizeAxes = Axes.Both;
                MainChildren = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Gray,
                                Alpha = 0.5f
                            },
                            new CytusESpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = @"LOADING",
                                Padding = new MarginPadding { Horizontal = 20, Vertical = 4 }
                            }
                        }
                    }
                };
            }
        }
    }
}
