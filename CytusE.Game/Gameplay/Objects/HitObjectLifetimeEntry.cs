using CytusE.Game.Gameplay.Objects.Drawables;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Performance;

namespace CytusE.Game.Gameplay.Objects
{
    public class HitObjectLifetimeEntry : LifetimeEntry
    {
        /// <summary>
        /// The <see cref="HitObject"/>.
        /// </summary>
        public readonly HitObject HitObject;

        private readonly IBindable<double> startTimeBindable = new BindableDouble();

        /// <summary>
        /// Creates a new <see cref="HitObjectLifetimeEntry"/>.
        /// </summary>
        /// <param name="hitObject">The <see cref="HitObject"/> to store the lifetime of.</param>
        public HitObjectLifetimeEntry(HitObject hitObject)
        {
            HitObject = hitObject;

            startTimeBindable.BindTo(HitObject.StartTimeBindable);
            startTimeBindable.BindValueChanged(_ => SetInitialLifetime(), true);

            // Subscribe to this event before the DrawableHitObject so that the local callback is invoked before the entry is re-applied as a result of DefaultsApplied.
            // This way, the DrawableHitObject can use OnApply() to overwrite the LifetimeStart that was set inside setInitialLifetime().
            //HitObject.DefaultsApplied += _ => SetInitialLifetime();
        }

        // The lifetime, as set by the hitobject.
        private double realLifetimeStart = double.MinValue;
        private double realLifetimeEnd = double.MaxValue;

        // This method is called even if `start == LifetimeStart` when `KeepAlive` is true (necessary to update `realLifetimeStart`).
        protected override void SetLifetimeStart(double start)
        {
            realLifetimeStart = start;
            if (!keepAlive)
                base.SetLifetimeStart(start);
        }

        protected override void SetLifetimeEnd(double end)
        {
            realLifetimeEnd = end;
            if (!keepAlive)
                base.SetLifetimeEnd(end);
        }

        private bool keepAlive;

        /// <summary>
        /// Whether the <see cref="HitObject"/> should be kept always alive.
        /// </summary>
        internal bool KeepAlive
        {
            set
            {
                if (keepAlive == value)
                    return;

                keepAlive = value;
                if (keepAlive)
                    SetLifetime(double.MinValue, double.MaxValue);
                else
                    SetLifetime(realLifetimeStart, realLifetimeEnd);
            }
        }

        /// <summary>
        /// A safe offset prior to the start time of <see cref="HitObject"/> at which it may begin displaying contents.
        /// By default, <see cref="HitObject"/>s are assumed to display their contents within 10 seconds prior to their start time.
        /// </summary>
        /// <remarks>
        /// This is only used as an optimisation to delay the initial application of the <see cref="HitObject"/> to a <see cref="DrawableHitObject"/>.
        /// A more accurate <see cref="LifetimeEntry.LifetimeStart"/> should be set on the hit object application, for further optimisation.
        /// </remarks>
        protected virtual double InitialLifetimeOffset => 10000;

        /// <summary>
        /// Set <see cref="LifetimeEntry.LifetimeStart"/> using <see cref="InitialLifetimeOffset"/>.
        /// </summary>
        internal void SetInitialLifetime() => LifetimeStart = HitObject.StartTime - InitialLifetimeOffset;
    }
}
