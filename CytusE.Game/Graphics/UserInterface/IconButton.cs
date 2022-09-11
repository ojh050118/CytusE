using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Graphics.UserInterface
{
    public class IconButton : CytusEAnimatedButton
    {
        public const float DEFAULT_ICON_SIZE = 30;

        private Color4? iconColour;

        public Color4 IconColour
        {
            get => iconColour ?? Color4.White;
            set
            {
                iconColour = value;
                icon.FadeColour(value);
            }
        }

        public IconUsage Icon
        {
            get => icon.Icon;
            set => icon.Icon = value;
        }

        public Vector2 IconScale
        {
            get => icon.Scale;
            set => icon.Scale = value;
        }

        private readonly SpriteIcon icon;

        public IconButton()
        {
            Size = new Vector2(DEFAULT_ICON_SIZE);

            Add(icon = new SpriteIcon
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(18)
            });
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            icon.Size = new Vector2(Size.X * 0.5f);
        }
    }
}

