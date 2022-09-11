using CytusE.Game.Gameplay.Objects.Pooling;
using osu.Framework.Allocation;

namespace CytusE.Game.Gameplay.Objects.Drawables
{
    [Cached(typeof(DrawableHitObject))]
    public class DrawableHitObject : PoolableDrawableWithLifetime<HitObjectLifetimeEntry>
    {
    }
}
