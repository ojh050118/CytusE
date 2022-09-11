using CytusE.Game.Graphics.Sprites;
using CytusE.Game.Screens.Select;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Screens
{
    public class MainScreen : CytusEScreen
    {
        public override bool CanExit => false;

        private GlowingSpriteText cytuse;
        private CytusESpriteText text;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                cytuse = new GlowingSpriteText
                {
                    Text = "CYTUS E!",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    GlowColour = Color4.DeepSkyBlue,
                    Font = FontUsage.Default.With(size: 96),
                    Spacing = new Vector2(40, 0)
                },
                text = new CytusESpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 80,
                    Alpha = 0,
                    Text = @"TOUCH SCREEN TO START",
                    Spacing = new Vector2(10, 0)
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            cytuse.FadeInFromZero(1000);
            text.FadeTo(0.5f, 1500).Then().FadeIn(1500).Loop();
        }

        protected override bool OnClick(ClickEvent e)
        {
            this.Push(new AuthorSelectScreen());

            return base.OnClick(e);
        }
    }
}
