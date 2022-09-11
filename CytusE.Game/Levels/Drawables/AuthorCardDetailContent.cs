using System.Collections.Generic;
using System.Linq;
using CytusE.Game.Graphics;
using CytusE.Game.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Levels.Drawables
{
    public class AuthorCardDetailContent : VisibilityContainer
    {
        protected override bool StartHidden => true;

        private readonly IEnumerable<Level> levels;
        private readonly Level selected;

        private LevelBackgroundSprite bg;
        private Container bgContent;
        private Container detail;

        private const float background_width = 400;
        private const float info_width = 200;
        private const float move_amount = 10;

        public const float WIDTH = background_width + move_amount + info_width;

        public AuthorCardDetailContent(IEnumerable<Level> levels, Level selected)
        {
            this.levels = levels;
            this.selected = selected;
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                bgContent = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 400,
                    Masking = true,
                    CornerRadius = 10,
                    Children = new Drawable[]
                    {
                        bg = new LevelBackgroundSprite(selected)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fill
                        },
                        new PlayButton()
                    }
                },
                detail = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    X = 410,
                    Child = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Y,
                        Width = 200,
                        Spacing = new Vector2(0, 5),
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            new SeparateLine(),
                            new CytusESpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = selected.LevelInfo.Author,
                                Padding = new MarginPadding { Vertical = 30 }
                            },
                            new SeparateLine(),
                            new CytusESpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = $"Lvls: {levels.Count().ToString()}"
                            },
                            new SeparateLine(),
                        }
                    }
                }
            };
        }

        protected override void PopIn()
        {
            bgContent.FadeIn();
            bg.ScaleTo(1.2f)
              .ScaleTo(1f, 400, Easing.OutQuint)
              .FadeInFromZero(400, Easing.OutQuint);

            detail.Delay(200)
                  .FadeInFromZero(400, Easing.OutQuint)
                  .MoveToX(400)
                  .MoveToX(410, 400, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            bgContent.FadeOut();
            bg.FadeOut();
            detail.FadeOut();
        }

        private class SeparateLine : Box
        {
            public SeparateLine()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.X;
                Height = 3;
                Colour = Color4.Gray;
                Alpha = 0.5f;
            }
        }

        private class PlayButton : Container
        {
            public PlayButton()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                AutoSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                InternalChild = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    BorderColour = Color4.White,
                    BorderThickness = 2.5f,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Transparent,
                            AlwaysPresent = true
                        },
                        new CytusESpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = "PLAY",
                            Spacing = new Vector2(12, 0),
                            Font = CytusEFont.Default.With(size: 36),
                            Padding = new MarginPadding { Horizontal = 36 }
                        }
                    }
                };
            }
        }
    }
}
