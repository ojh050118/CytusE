using osu.Framework.Platform;
using osu.Framework;
using CytusE.Game;

namespace CytusE.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"CytusE"))
            using (osu.Framework.Game game = new CytusEGame())
                host.Run(game);
        }
    }
}
