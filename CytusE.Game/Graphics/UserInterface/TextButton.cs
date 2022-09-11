using CytusE.Game.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace CytusE.Game.Graphics.UserInterface
{
    public class TextButton : ClickableContainer
    {
        public string Text
        {
            get => text.Text.ToString();
            set => text.Text = value;
        }

        private readonly CytusESpriteText text;
        private readonly Box hover;

        public TextButton(bool isPositive)
        {
            Masking = true;
            BorderColour = Color4.White.Opacity(0.7f);
            BorderThickness = isPositive ? 0 : 3;
            RelativeSizeAxes = Axes.X;
            Height = 40;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = (isPositive ? Color4.White : Color4.Black).Opacity(0.7f)
                },
                text = new CytusESpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = CytusEFont.GetFont(weight: FontWeight.Bold),
                    Colour = isPositive ? Color4.Black : Color4.White
                },
                hover = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White.Opacity(0.3f),
                    Alpha = 0
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            CornerRadius = DrawHeight / 7;
        }

        protected override bool OnHover(HoverEvent e)
        {
            hover.FadeIn(500, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            hover.FadeOut(500, Easing.OutQuint);
        }
    }
}
