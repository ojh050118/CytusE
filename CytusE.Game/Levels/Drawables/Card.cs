using System;
using CytusE.Game.Graphics.Containers;
using CytusE.Game.Graphics.UserInterface;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace CytusE.Game.Levels.Drawables
{
    public abstract class Card : CytusEClickableContainer, IStateful<SelectionState>
    {
        private SelectionState state;

        public SelectionState State
        {
            get => state;
            set
            {
                if (state == value)
                    return;

                state = value;
                StateChanged?.Invoke(value);
            }
        }

        public event Action<SelectionState> StateChanged;

        protected Box Dim { get; }

        protected Card()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            Content.RelativeSizeAxes = Axes.Y;
            Content.AutoSizeAxes = Axes.X;
            StateChanged += UpdateState;

            Dim = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
                Alpha = 0
            };
        }

        protected virtual void UpdateState(SelectionState state)
        {
        }

        protected override bool OnHover(HoverEvent e)
        {
            Dim?.FadeTo(0.2f, 200, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            Dim?.FadeOut(200, Easing.OutQuint);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Dim?.FadeTo(0.6f, 200, Easing.OutQuint);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);

            Dim?.FadeOut(200, Easing.OutQuint);
        }

        protected override bool OnClick(ClickEvent e)
        {
            State = SelectionState.Selected;

            return base.OnClick(e);
        }
    }
}
