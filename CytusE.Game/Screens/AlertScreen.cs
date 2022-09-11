using CytusE.Game.Graphics;
using CytusE.Game.Graphics.Sprites;
using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Screens
{
    public class AlertScreen : CytusEScreen
    {
        public override bool CanExit => false;

        private FillFlowContainer content;

        private MainScreen mainScreen;

        private LoadingLayer loading;
        private ScheduledDelegate loadingShow;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                content = new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 20),
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(120),
                            Icon = FontAwesome.Solid.Headset,
                        },
                        new CytusESpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = "Headphones are recommended for the best play experience",
                            Font = CytusEFont.GetFont(size: 48)
                        }
                    }
                }
            };
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);

            LoadComponentAsync(mainScreen = new MainScreen());

            LoadComponentAsync(loading = new LoadingLayer(), _ =>
            {
                loading.Hide();
                AddInternal(loading);
                loadingShow = Scheduler.AddDelayed(loading.Show, 4500);
            });

            content.FadeInFromZero(1000).Delay(2500).FadeOut(1000);
            Scheduler.AddDelayed(checkIsLoaded, 4500);
        }

        private void checkIsLoaded()
        {
            if (mainScreen.LoadState != LoadState.Ready)
            {
                Schedule(checkIsLoaded);
                return;
            }

            loadingShow?.Cancel();

            if (loading.State.Value == Visibility.Visible)
            {
                loading.Hide();
                Scheduler.AddDelayed(() => this.Push(mainScreen), LoadingSpinner.TRANSITION_DURATION);
            }
            else
            {
                this.Push(mainScreen);
            }
        }
    }
}
