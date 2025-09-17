// RaylibShapeDrawer.cs
using System.Drawing;
using BattleStars.Utility;

namespace BattleStars.Shapes
{
    public class RaylibShapeDrawer : IShapeDrawer
    {
        private readonly IRaylibGraphics _graphics;

        public RaylibShapeDrawer(IRaylibGraphics graphics)
        {
            ArgumentNullException.ThrowIfNull(graphics);
            _graphics = graphics;
        }

        public void DrawRectangle(PositionalVector2 topleft, PositionalVector2 bottomright, Color color) =>
            _graphics.DrawRectangle(topleft, bottomright - topleft, color);

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) =>
            _graphics.DrawTriangle(p1, p2, p3, color);

        public void DrawCircle(PositionalVector2 center, float radius, Color color) =>
            _graphics.DrawCircle(center, radius, color);
    }
}
