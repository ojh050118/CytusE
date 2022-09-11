using System.Collections.Generic;
using CytusE.Game.Levels;
using CytusE.Game.Levels.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace CytusE.Game.Tests.Visual.Levels
{
    public class TestSceneAuthorCard : CytusETestScene
    {
        [Resolved]
        private LevelStore levels { get; set; }

        private readonly List<Level> authorLevels = new List<Level>();

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var name in levels.GetAvailableResources())
            {
                var level = levels.Get(name);

                if (level.LevelInfo.Author == "Cytus E!")
                    authorLevels.Add(level);
            }

            Add(new Container
            {
                Padding = new MarginPadding(20),
                RelativeSizeAxes = Axes.Both,
                Child = new AuthorCard(authorLevels)
            });
        }
    }
}


