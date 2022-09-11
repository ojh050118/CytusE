using CytusE.Game.Levels.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Screens.Select
{
    public class AddCard : Card
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            CornerRadius = 10;

            Content.AutoSizeAxes = Axes.None;
            Content.RelativeSizeAxes = Axes.Y;
            Content.Width = 200;

            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(100, 100, 100, 122)
                },
                Dim,
                new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(30),
                    Icon = FontAwesome.Solid.Plus
                }
            });
        }
    }
}
