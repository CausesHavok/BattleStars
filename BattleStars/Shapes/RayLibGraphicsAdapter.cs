using BattleStars.Utility;
using Raylib_cs;

namespace BattleStars.Shapes
{
    public class RaylibGraphicsAdapter : IRaylibGraphics
    {
        private static Color ToRaylibColor(System.Drawing.Color color) =>
            new Color(color.R, color.G, color.B, color.A);

        public void DrawRectangle(PositionalVector2 topLeft, PositionalVector2 size, System.Drawing.Color color)
        {
            Raylib.DrawRectangleV(topLeft, size, ToRaylibColor(color));
        }

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, System.Drawing.Color color)
        {
            Raylib.DrawTriangle(p1, p2, p3, ToRaylibColor(color));
        }

        public void DrawCircle(PositionalVector2 center, float radius, System.Drawing.Color color)
        {
            Raylib.DrawCircleV(center, radius, ToRaylibColor(color));
        }
    }
}
