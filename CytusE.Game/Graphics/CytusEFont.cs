using osu.Framework.Graphics.Sprites;

namespace CytusE.Game.Graphics
{
    public static class CytusEFont
    {
        public const float DEFAULT_FONT_SIZE = 24;

        public static FontUsage Default => GetFont();

        public static FontUsage GetFont(Typeface typeface = Typeface.Electrolize, float size = DEFAULT_FONT_SIZE, FontWeight weight = FontWeight.Medium, bool italics = false, bool fixedWidth = false)
        {
            string familyString = GetFamilyString(typeface);
            return new FontUsage(familyString, size, GetWeightString(familyString, weight), getItalics(italics), fixedWidth);
        }

        private static bool getItalics(in bool italicsRequested)
        {
            return false;
        }

        public static string GetFamilyString(Typeface typeface)
        {
            switch (typeface)
            {
                case Typeface.Electrolize:
                    return @"Electrolize";
            }

            return null;
        }

        public static string GetWeightString(string family, FontWeight weight)
        {
            return weight.ToString();
        }
    }

    public static class CytusEFontExtensions
    {
        public static FontUsage With(this FontUsage usage, Typeface? typeface = null, float? size = null, FontWeight? weight = null, bool? italics = null, bool? fixedWidth = null)
        {
            string familyString = typeface != null ? CytusEFont.GetFamilyString(typeface.Value) : usage.Family;
            string weightString = weight != null ? CytusEFont.GetWeightString(familyString, weight.Value) : usage.Weight;

            return usage.With(familyString, size, weightString, italics, fixedWidth);
        }
    }

    public enum Typeface
    {
        Electrolize
    }

    public enum FontWeight
    {
        Light = 300,
        Regular = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        Black = 900
    }
}
