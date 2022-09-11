using System;
using System.Diagnostics;
using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Pooling;

namespace CytusE.Game.Gameplay.Objects.Pooling
{
    public class PoolableDrawableWithLifetime<TEntry> : PoolableDrawable where TEntry : LifetimeEntry
    {
        [CanBeNull]
        private TEntry entry;

        [CanBeNull]
        public TEntry Entry
        {
            get => entry;
            set
            {
                if (LoadState == LoadState.NotLoaded)
                    entry = value;
            }
        }

        protected bool HasEntryApplied { get; private set; }

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                if (Entry == null && LifetimeStart != value)
                    throw new InvalidOperationException($"Cannot modify lifetime of {nameof(PoolableDrawableWithLifetime<TEntry>)} when entry is not set");

                if (Entry != null)
                    Entry.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                if (Entry == null && LifetimeEnd != value)
                    throw new InvalidOperationException($"Cannot modify lifetime of {nameof(PoolableDrawableWithLifetime<TEntry>)} when entry is not set");

                if (Entry != null)
                    Entry.LifetimeEnd = value;
            }
        }

        public override bool RemoveWhenNotAlive => false;
        public override bool RemoveCompletedTransforms => false;

        protected PoolableDrawableWithLifetime(TEntry initialEntry = null)
        {
            Entry = initialEntry;
        }

        protected override void LoadAsyncComplete()
        {
            base.LoadAsyncComplete();

            // Apply the initial entry.
            if (Entry != null && !HasEntryApplied)
                apply(Entry);
        }

        public void Apply(TEntry entry)
        {
            if (LoadState == LoadState.Loading)
                throw new InvalidOperationException($"Cannot apply a new {nameof(TEntry)} while currently loading.");

            apply(entry);
        }

        protected sealed override void FreeAfterUse()
        {
            base.FreeAfterUse();

            // We preserve the existing entry in case we want to move a non-pooled drawable between different parent drawables.
            if (HasEntryApplied && IsInPool)
                free();
        }

        protected virtual void OnApply(TEntry entry)
        {
        }

        protected virtual void OnFree(TEntry entry)
        {
        }

        private void apply(TEntry entry)
        {
            if (HasEntryApplied)
                free();

            this.entry = entry;
            entry.LifetimeChanged += setLifetimeFromEntry;
            setLifetimeFromEntry(entry);

            OnApply(entry);

            HasEntryApplied = true;
        }

        private void free()
        {
            Debug.Assert(Entry != null && HasEntryApplied);

            OnFree(Entry);

            Entry.LifetimeChanged -= setLifetimeFromEntry;
            entry = null;
            base.LifetimeStart = double.MinValue;
            base.LifetimeEnd = double.MaxValue;

            HasEntryApplied = false;
        }

        private void setLifetimeFromEntry(LifetimeEntry entry)
        {
            Debug.Assert(entry == Entry);
            Debug.Assert(entry != null);
            base.LifetimeStart = entry.LifetimeStart;
            base.LifetimeEnd = entry.LifetimeEnd;
        }
    }
}
