using System;
using CytusE.Game.Levels;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Threading;

namespace CytusE.Game.Overlays
{
    public class MusicController : CompositeDrawable
    {
        [Resolved]
        private IBindable<Level> level { get; set; }

        public event Action<Level> TrackChanged;

        public DrawableTrack CurrentTrack { get; private set; } = new DrawableTrack(new TrackVirtual(1000));

        public bool UserPauseRequested { get; private set; }

        public bool IsPlaying => CurrentTrack.IsRunning;

        public bool TrackLoaded => CurrentTrack.TrackLoaded;

        private ScheduledDelegate seekDelegate;

        private Level current;

        [BackgroundDependencyLoader]
        private void load()
        {
            level.BindValueChanged(levelChanged, true);
        }

        public void ReloadCurrentTrack()
        {
            changeTrack();
            TrackChanged?.Invoke(current);
        }

        public void SeekTo(double position)
        {
            seekDelegate?.Cancel();
            seekDelegate = Schedule(() =>
            {
                if (!level.Disabled)
                    CurrentTrack.Seek(position);
            });
        }

        public bool Play(bool restart = false, bool requestedByUser = false)
        {
            if (requestedByUser)
                UserPauseRequested = false;

            if (restart)
                CurrentTrack.RestartAsync();
            else if (!IsPlaying)
                CurrentTrack.StartAsync();

            return true;
        }

        public void Stop(bool requestedByUser = false)
        {
            UserPauseRequested |= requestedByUser;

            if (CurrentTrack.IsRunning)
                CurrentTrack.StopAsync();
        }

        public bool TogglePause()
        {
            if (CurrentTrack.IsRunning)
                Stop(true);
            else
                Play(requestedByUser: true);

            return true;
        }

        private void restartTrack()
        {
            Schedule(() => CurrentTrack.RestartAsync());
        }

        private void levelChanged(ValueChangedEvent<Level> e) => changeLevel(e.NewValue);

        private void changeLevel(Level newLevel)
        {
            if (newLevel == current)
                return;

            var lastLevel = current;

            current = newLevel;

            if (lastLevel?.Track != null)
                changeTrack();

            TrackChanged?.Invoke(current);

            if (level.Value != current && level is Bindable<Level> lvl)
                lvl.Value = current;
        }

        private void changeTrack()
        {
            var queuedTrack = getQueuedTrack();

            var lastTrack = CurrentTrack;
            CurrentTrack = queuedTrack;

            Schedule(() =>
            {
                lastTrack.VolumeTo(0, 500, Easing.Out).Expire();

                if (queuedTrack == CurrentTrack)
                {
                    AddInternal(queuedTrack);
                    queuedTrack.VolumeTo(0).Then().VolumeTo(1, 300, Easing.Out);
                }
                else
                {
                    queuedTrack.Dispose();
                }
            });
        }

        private DrawableTrack getQueuedTrack()
        {
            var queuedTrack = new DrawableTrack(current.Track);
            queuedTrack.Completed += () => onTrackCompleted(current);

            return queuedTrack;
        }

        private void onTrackCompleted(Level level)
        {
            if (current != level)
                return;

            Play(true);
        }
    }
}
