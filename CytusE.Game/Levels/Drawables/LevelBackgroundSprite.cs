using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace CytusE.Game.Levels.Drawables
{
    public class LevelBackgroundSprite : Sprite
    {
        private readonly Level level;

        public LevelBackgroundSprite(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            this.level = level;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (level.Background != null)
                Texture = level.Background;
        }
    }
}
