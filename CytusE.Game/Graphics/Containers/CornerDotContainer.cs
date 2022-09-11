using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Graphics.Containers
{
    public class CornerDotContainer : Container
    {
        public const float DOT_WIDTH = 4;
        public const float DOT_HEIGHT = 1;

        public Color4 DotColour = Color4.White;

        public bool ShowLine
        {
            get => showLine;
            set
            {
                showLine = value;
                lines.Alpha = value ? 1 : 0;
            }
        }

        private bool showLine = true;

        private readonly Container lines;

        protected override Container<Drawable> Content => content;

        private readonly Container content = new Container { RelativeSizeAxes = Axes.Both };

        private readonly Container mainContent;

        public new Axes RelativeSizeAxes
        {
            get => base.RelativeSizeAxes;
            set
            {
                base.RelativeSizeAxes = value;
                content.RelativeSizeAxes = value;
                mainContent.RelativeSizeAxes = value;
            }
        }

        public new Axes AutoSizeAxes
        {
            get => base.AutoSizeAxes;
            set
            {
                base.AutoSizeAxes = value;
                content.AutoSizeAxes = value;
                mainContent.AutoSizeAxes = value;
            }
        }

        public IReadOnlyList<Drawable> MainChildren
        {
            get => mainContent?.Children;
            set => mainContent.Children = value;
        }

        public CornerDotContainer()
        {
            InternalChildren = new Drawable[]
            {
                content
            };

            content.Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Dots",
                    Children = new Drawable[]
                    {
                        new Circle
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding { Right = DOT_WIDTH * 2 },
                            Colour = DotColour,
                            Size = new Vector2(DOT_WIDTH)
                        },
                        new Circle
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.CentreLeft,
                            Margin = new MarginPadding { Left = DOT_WIDTH * 2 },
                            Colour = DotColour,
                            Size = new Vector2(DOT_WIDTH)
                        },
                        new Circle
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding { Right = DOT_WIDTH * 2 },
                            Colour = DotColour,
                            Size = new Vector2(DOT_WIDTH)
                        },
                        new Circle
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.CentreLeft,
                            Margin = new MarginPadding { Left = DOT_WIDTH * 2 },
                            Colour = DotColour,
                            Size = new Vector2(DOT_WIDTH)
                        }
                    },
                },
                lines = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Lines",
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Height = DOT_HEIGHT,
                            Colour = DotColour
                        },
                        new Box
                        {
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Height = DOT_HEIGHT,
                            Colour = DotColour
                        }
                    }
                },
                mainContent = new Container
                {
                    RelativeSizeAxes = Content.RelativeSizeAxes,
                    AutoSizeAxes = Content.AutoSizeAxes,
                    Name = "Content",
                }
            };
        }
    }
}
