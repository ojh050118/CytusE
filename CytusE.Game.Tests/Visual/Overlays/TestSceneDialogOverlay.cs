using CytusE.Game.Overlays;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace CytusE.Game.Tests.Visual.Overlays
{
    public class TestSceneDialogOverlay : CytusETestScene
    {
        public TestSceneDialogOverlay()
        {
            DialogOverlay dialog;

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
            Add(dialog = new DialogOverlay
            {
                Title = "Warning",
                Description = "Are you sure you want to quit?\nUnsaved content is gone!\nWhat would you do?"
            });
            AddStep("Hide", dialog.Hide);
            AddStep("Add buttons", () => dialog.ButtonLayout = new[]
            {
                new GridButtonInfo
                {
                    Action = dialog.Hide,
                    Row = 0,
                    Column = 0,
                    IsPositive = true,
                    Text = "Save and exit"
                },
                new GridButtonInfo
                {
                    Action = Show,
                    Row = 1,
                    Column = 0,
                    IsPositive = true,
                    Text = "Yes"
                },
                new GridButtonInfo
                {
                    Action = dialog.Hide,
                    Row = 1,
                    Column = 1,
                    IsPositive = false,
                    Text = "No"
                }
            });
            AddStep("Show", dialog.Show);
        }
    }
}

