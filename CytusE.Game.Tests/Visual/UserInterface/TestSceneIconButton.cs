using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Tests.Visual.UserInterface
{
    public class TestSceneIconButton : CytusETestScene
    {
        public TestSceneIconButton()
        {
            IconButton enabledButton, disabledButton;
            BindableBool enabled = new BindableBool(true);
            BindableBool disabled = new BindableBool(false);

            Add(new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
            Add(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                Content = new[]
                {
                    new Drawable[]
                    {
                        new IconButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(90),
                            Icon = FontAwesome.Solid.Cog,
                            IconColour = Color4.DeepSkyBlue
                        },
                        new IconButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(90),
                            Icon = FontAwesome.Solid.List,
                            IconColour = Color4.Red
                        }
                    },
                    new Drawable[]
                    {
                        enabledButton = new IconButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(90),
                            Icon = FontAwesome.Solid.Circle,
                            IconColour = Color4.Yellow
                        },
                        disabledButton = new IconButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(90),
                            Icon = FontAwesome.Solid.Barcode,
                            IconColour = Color4.Green
                        }
                    }
                }
            });

            enabled.BindTo(enabledButton.Enabled);
            disabled.BindTo(disabledButton.Enabled);

            AddStep("Toggle disable button1", () => enabled.Value = !enabled.Value);
            AddStep("Toggle disable button2", () => disabled.Value = !disabled.Value);
            AddSliderStep("button1 size", 30, 150, 90, v => enabledButton.Size = new Vector2(v));
        }
    }
}


