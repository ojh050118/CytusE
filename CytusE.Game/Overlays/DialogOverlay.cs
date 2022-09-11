using System;
using System.Collections.Generic;
using System.Linq;
using CytusE.Game.Graphics;
using CytusE.Game.Graphics.Containers;
using CytusE.Game.Graphics.Sprites;
using CytusE.Game.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace CytusE.Game.Overlays
{
    public class DialogOverlay : CytusEFocusedOverlayContainer
    {
        public string Title
        {
            get => title.Text.ToString();
            set => title.Text = value;
        }

        public string Description
        {
            set => description.Text = value;
        }

        public IReadOnlyList<GridButtonInfo> ButtonLayout
        {
            get => buttonLayout;
            set
            {
                buttonLayout = value;

                if (buttonContainer == null)
                    return;

                buttonContainer.Clear();
                buttonContainer.Children = createButtons();
            }
        }

        private IReadOnlyList<GridButtonInfo> buttonLayout;

        private readonly CytusESpriteText title;
        private readonly TextFlowContainer description;

        private readonly FillFlowContainer buttonContainer;

        private readonly CornerDotContainer content;

        public DialogOverlay()
        {
            base.Content.Add(content = new CornerDotContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Width = 0.4f,
                Height = 0.6f,
                MainChildren = new Drawable[]
                {
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Vertical = 10 },
                        Masking = true,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Black,
                                Alpha = 0.6f
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Black
                                    },
                                    title = new CytusESpriteText
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = "Title",
                                        Font = CytusEFont.GetFont(size: 40, weight: FontWeight.Bold),
                                        Padding = new MarginPadding { Vertical = 20 }
                                    }
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding { Top = 80, Horizontal = 30, Bottom = 30 },
                                Children = new Drawable[]
                                {
                                    description = new TextFlowContainer(t => t.Font = CytusEFont.Default)
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.BottomCentre,
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        TextAnchor = Anchor.CentreLeft,
                                        Text = "Description\na\nb"
                                    },
                                    buttonContainer = new FillFlowContainer
                                    {
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Direction = FillDirection.Vertical,
                                        Spacing = new Vector2(0, 10)
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        public void Push(IReadOnlyList<GridButtonInfo> buttonLayout)
        {
            ButtonLayout = buttonLayout;

            buttonContainer.Clear();
            buttonContainer.Children = createButtons();

            Show();
        }

        protected override void PopIn()
        {
            base.PopIn();

            content.ResizeTo(new Vector2(0.05f, 0f)).FadeIn()
                   .ResizeWidthTo(0.4f, 300, Easing.OutQuint).Then()
                   .ResizeHeightTo(0.6f, 300, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            content.ResizeHeightTo(0f, 300, Easing.OutQuint).Then()
                   .ResizeWidthTo(0.05f, 300, Easing.OutQuint).Then()
                   .FadeOut(75);
        }

        private IReadOnlyList<Drawable> createButtons()
        {
            List<Drawable> buttons = new List<Drawable>();
            int lastRow = -1;

            if (ButtonLayout == null)
                return buttons;

            foreach (var r in ButtonLayout.OrderBy(info => info.Row))
            {
                if (lastRow == r.Row)
                    continue;

                buttons.Add(createButtonGrid(r.Row));
                lastRow = (int)r.Row;
            }

            return buttons;
        }

        private Dimension[] getColumnDimension(uint row)
        {
            List<Dimension> column = new List<Dimension>();

            foreach (var r in ButtonLayout.Where(i => i.Row == row))
            {
                column.Add(new Dimension());

                if (ButtonLayout.Any(i => i.Column > r.Column))
                    column.Add(new Dimension(GridSizeMode.AutoSize));
            }

            return column.ToArray();
        }

        private GridContainer createButtonGrid(uint row)
        {
            return new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize)
                },
                ColumnDimensions = getColumnDimension(row),
                Content = createGridContent(row)
            };
        }

        private Drawable[][] createGridContent(uint row)
        {
            List<Drawable[]> grid = new List<Drawable[]>();
            var currentRow = ButtonLayout.Where(info => info.Row == row);
            var currentColumn = currentRow.OrderBy(info => info.Column).ToArray();
            var column = new List<Drawable>();

            for (int j = 0; j < currentColumn.Length; j++)
            {
                var info = currentColumn[j];
                column.Add(new TextButton(info.IsPositive)
                {
                    Text = info.Text,
                    Action = info.Action,
                });

                // 버튼사이의 간격을 추가합니다.
                if (j < currentColumn.Length - 1)
                {
                    column.Add(new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 10
                    });
                }
            }

            grid.Add(column.ToArray());

            return grid.ToArray();
        }
    }

    public class GridButtonInfo
    {
        public uint Row;
        public uint Column;
        public string Text;
        public bool IsPositive;
        public Action Action;
    }
}
