using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace CytusE.Game.Screens.Select
{
    public class Footer : Container
    {
        public const float BUTTON_SIZE = 75;

        private const float padding = 20;

        private readonly FillFlowContainer<IconButton> left;
        private readonly FillFlowContainer<IconButton> right;

        public Footer()
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding(padding);
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize)
                },
                Content = new[]
                {
                    new[]
                    {
                        left = new FillFlowContainer<IconButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal
                        },
                        right = new FillFlowContainer<IconButton>
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal
                        }
                    }
                }
            };
        }

        public void AddButton(IconButton button, bool left)
        {
            button.Size = new Vector2(BUTTON_SIZE);

            if (left)
                this.left.Add(button);
            else
                right.Add(button);
        }
    }
}
