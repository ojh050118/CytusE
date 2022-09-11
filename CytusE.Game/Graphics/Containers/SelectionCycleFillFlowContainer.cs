using System;
using System.Diagnostics;
using CytusE.Game.Graphics.UserInterface;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace CytusE.Game.Graphics.Containers
{
    public class SelectionCycleFillFlowContainer<T> : FillFlowContainer<T> where T : Drawable, IStateful<SelectionState>
    {
        public T Selected => (selectedIndex >= 0 && selectedIndex < Count) ? this[selectedIndex.Value] : null;

        private int? selectedIndex;

        public void SelectNext()
        {
            if (!selectedIndex.HasValue || selectedIndex == Count - 1)
                setSelected(0);
            else
                setSelected(selectedIndex.Value + 1);
        }

        public void SelectPrevious()
        {
            if (!selectedIndex.HasValue || selectedIndex == 0)
                setSelected(Count - 1);
            else
                setSelected(selectedIndex.Value - 1);
        }

        public void Deselect() => setSelected(null);

        public void Select(T item)
        {
            int newIndex = IndexOf(item);

            if (newIndex < 0)
                setSelected(null);
            else
                setSelected(IndexOf(item));
        }

        public override void Add(T drawable)
        {
            base.Add(drawable);

            Debug.Assert(drawable != null);

            drawable.StateChanged += state => selectionChanged(drawable, state);
        }

        public override bool Remove(T drawable)
            => throw new NotSupportedException($"Cannot remove drawables from {nameof(SelectionCycleFillFlowContainer<T>)}");

        private void setSelected(int? value)
        {
            if (selectedIndex == value)
                return;

            if (selectedIndex.HasValue)
                this[selectedIndex.Value].State = SelectionState.NotSelected;

            selectedIndex = value;

            if (selectedIndex.HasValue)
                this[selectedIndex.Value].State = SelectionState.Selected;
        }

        private void selectionChanged(T drawable, SelectionState state)
        {
            if (state == SelectionState.NotSelected)
                Deselect();
            else
                Select(drawable);
        }
    }
}
