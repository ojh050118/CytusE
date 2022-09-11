using System.Collections.Generic;
using System.Linq;
using CytusE.Game.Graphics.Sprites;
using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Threading;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Levels.Drawables
{
    public class AuthorCard : Card
    {
        public readonly IEnumerable<Level> Levels;
        public Level Selected { get; private set; }

        public const float TRANSITION_DURATION = 400;

        private Container sizing;
        private Container card;
        private AuthorCardDetailContent detail;

        private ScheduledDelegate detailShow;

        public AuthorCard(IEnumerable<Level> levels)
        {
            Levels = levels;

            if (levels.Any())
                Selected = Levels.ToArray()[RNG.Next(0, Levels.Count())];
        }

        [BackgroundDependencyLoader]
        private void load(DummyLevel dummyLevel)
        {
            if (!Levels.Any())
                Selected = dummyLevel;

            AddRange(new Drawable[]
            {
                sizing = new Container(),
                new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Y,
                    Width = 200,
                    Child = card = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Masking = true,
                        CornerRadius = 10,
                        Children = new Drawable[]
                        {
                            new LevelBackgroundSprite(Selected)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fill
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 100,
                                Colour = ColourInfo.GradientVertical(Color4.Black.Opacity(0.7f), Color4.Black.Opacity(0))
                            },
                            Dim,
                            new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Margin = new MarginPadding { Top = 5 },
                                Spacing = new Vector2(0, 5),
                                Children = new Drawable[]
                                {
                                    new CytusESpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Text = Selected.LevelInfo.Author
                                    },
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 4,
                                        Colour = Color4.Gray,
                                        Alpha = 0.5f
                                    },
                                    new CytusESpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Text = $"Lvls: {Levels.Count().ToString()}"
                                    }
                                }
                            },
                        }
                    }
                },
                detail = new AuthorCardDetailContent(Levels, Selected)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft
                }
            });
        }

        protected override void UpdateState(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.NotSelected:
                    sizing.ResizeWidthTo(0);
                    detailShow?.Cancel();
                    detail.Hide();
                    card.FinishTransforms();
                    card.FadeInFromZero(TRANSITION_DURATION)
                        .ScaleTo(1);
                    break;

                case SelectionState.Selected:
                    sizing.ResizeWidthTo(AuthorCardDetailContent.WIDTH);
                    card.FinishTransforms();
                    card.ScaleTo(1.5f, TRANSITION_DURATION, Easing.OutQuint)
                        .FadeOut(TRANSITION_DURATION, Easing.OutQuint);
                    detailShow = Scheduler.AddDelayed(detail.Show, TRANSITION_DURATION);
                    break;
            }
        }
    }
}
