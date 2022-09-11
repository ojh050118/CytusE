using CytusE.Game.Graphics.UserInterface;
using CytusE.Game.Screens.Select;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace CytusE.Game.Screens.Setting
{
    public class SettingsScreen : CytusEScreen
    {
        private Footer footer;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        new[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both
                            }
                        },
                        new Container[]
                        {
                            footer = new Footer()
                        }
                    }
                }
            };

            footer?.AddButton(new IconButton
            {
                Action = OnExit,
                Icon = FontAwesome.Solid.ArrowLeft
            }, true);
        }
    }
}
