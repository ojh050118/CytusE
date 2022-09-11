using CytusE.Game.Graphics.Backgrounds;
using CytusE.Game.Graphics.UserInterface;
using CytusE.Game.Levels;
using CytusE.Game.Levels.Drawables;
using CytusE.Game.Overlays;
using CytusE.Game.Screens.Setting;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Screens.Select
{
    public class AuthorSelectScreen : CytusEScreen
    {
        private Container<Background> backgroundContainer;
        private Background background;
        private LevelCarousel carousel;

        private Footer footer;

        [Resolved]
        private Bindable<Level> level { get; set; }

        [Resolved]
        private MusicController music { get; set; }

        [Resolved]
        private DummyLevel dummyLevel { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                backgroundContainer = new Container<Background>
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        new[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding { Top = 95, Bottom = 20 },
                                Child = carousel = new LevelCarousel()
                            }
                        },
                        new Container[]
                        {
                            footer = new Footer()
                        }
                    }
                }
            };

            if (footer != null)
            {
                footer.AddButton(new IconButton
                {
                    Icon = FontAwesome.Solid.ArrowLeft,
                    Action = OnExit
                }, true);

                footer.AddButton(new IconButton
                {
                    Icon = FontAwesome.Solid.List,
                    Action = () => this.Push(new SettingsScreen())
                }, false);
            }

            carousel.AddAddCard(new AddCard
            {
                Action = carousel.Deselect
            });
            carousel.Selected.ValueChanged += onLevelChanged;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            background?.BlurTo(new Vector2(20));
        }

        private void onLevelChanged(ValueChangedEvent<AuthorCard> e)
        {
            level.Value = e.NewValue == null ? dummyLevel : e.NewValue?.Selected;

            switchBackground(new Background(e.NewValue?.Selected?.Background));
            music.Play(requestedByUser: true);
        }

        private void switchBackground(Background b)
        {
            float newDepth = 0;

            if (background != null)
            {
                newDepth = background.Depth + 1;
                background.FinishTransforms();
                background.FadeOut(250);
                background.Expire();
            }

            b.BlurTo(new Vector2(20));
            b.Depth = newDepth;
            background = b;
            backgroundContainer.Add(b);
        }

        public override void OnExit()
        {
            if (carousel.Selected.Value != null)
                carousel.Deselect();
            else
                base.OnExit();
        }
    }
}
