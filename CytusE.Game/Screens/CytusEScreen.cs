using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace CytusE.Game.Screens
{
    public class CytusEScreen : Screen
    {
        public virtual bool CanExit => true;

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);

            this.FadeInFromZero(500, Easing.Out);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            this.FadeOut(500, Easing.OutQuint);

            return base.OnExiting(e);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            base.OnSuspending(e);

            this.FadeOut(500, Easing.OutQuint);
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);

            this.FadeInFromZero(500, Easing.OutQuint);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    OnExit();
                    break;
            }

            return base.OnKeyDown(e);
        }

        public virtual void OnExit()
        {
            if (!CanExit)
                return;

            this.Exit();
        }
    }
}
