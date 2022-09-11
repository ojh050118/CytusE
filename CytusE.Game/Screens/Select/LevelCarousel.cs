using System.Collections.Generic;
using System.Linq;
using CytusE.Game.Graphics.Containers;
using CytusE.Game.Graphics.UserInterface;
using CytusE.Game.Levels;
using CytusE.Game.Levels.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace CytusE.Game.Screens.Select
{
    public class LevelCarousel : Container
    {
        public readonly Bindable<AuthorCard> Selected = new Bindable<AuthorCard>();

        private BasicScrollContainer scroll;
        private SelectionCycleFillFlowContainer<AuthorCard> content;
        private AddCard addCard;

        [BackgroundDependencyLoader]
        private void load(LevelManager levelManager)
        {
            RelativeSizeAxes = Axes.Both;
            Child = scroll = new BasicScrollContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Horizontal = 95 },
                ScrollbarVisible = false,
                Child = content = new SelectionCycleFillFlowContainer<AuthorCard>
                {
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(40, 0),
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    AutoSizeDuration = 100,
                    AutoSizeEasing = Easing.Out,
                    Children = createAuthorCard(levelManager.GetAvailableLevels())
                }
            };
        }

        public void AddAddCard(AddCard card)
        {
            addCard = card;
            card.X = content.Width + 40;
            scroll.Add(card);
        }

        public void Deselect()
        {
            Selected.Value = null;
            content.Deselect();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (addCard != null)
                addCard.X = content.Width + (content.Count == 0 ? 0 : 40);
        }

        private IReadOnlyList<AuthorCard> createAuthorCard(IReadOnlyList<Level> levels)
        {
            var lastAuthor = string.Empty;
            List<string> authors = new List<string>();

            List<AuthorCard> authorCard = new List<AuthorCard>();

            foreach (var level in levels.OrderBy(l => l.LevelInfo.Author))
            {
                if (lastAuthor != level.LevelInfo.Author)
                {
                    authors.Add(level.LevelInfo.Author);
                    lastAuthor = level.LevelInfo.Author;
                }
            }

            foreach (var author in authors)
            {
                var authorLevels = levels.Where(l => l.LevelInfo.Author == author);

                var card = new AuthorCard(authorLevels);
                card.StateChanged += state =>
                {
                    if (state == SelectionState.Selected)
                        Selected.Value = card;
                };

                authorCard.Add(card);
            }

            return authorCard;
        }
    }
}
