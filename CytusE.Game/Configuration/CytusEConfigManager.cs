using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace CytusE.Game.Configuration
{
    public class CytusEConfigManager : IniConfigManager<CytusESetting>
    {
        protected override string Filename => @"CytusE.ini";

        public CytusEConfigManager(Storage storage)
            : base(storage)
        {
        }
    }

    public enum CytusESetting
    {
    }
}
