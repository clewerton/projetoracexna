using Microsoft.Xna.Framework;

namespace TangoGames.RoadFighter.Utils
{
    public static class ColorUtils
    {
        public static Color WithAlpha(this Color color, int alpha)
        {
            return new Color(color.R, color.G, color.B, alpha);
        }
    }
}
