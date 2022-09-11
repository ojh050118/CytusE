using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace CytusE.Game.Gameplay.UI
{
    public class Playfield : CompositeDrawable
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);

        public Playfield()
        {
            RelativeSizeAxes = Axes.Both;
        }
    }
}
