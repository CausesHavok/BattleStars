// IRaylibGraphics.cs
using System.Drawing;
using BattleStars.Utility;

namespace BattleStars.Shapes
{
    public interface IRaylibGraphics
    {
        void DrawRectangle(PositionalVector2 topLeft, PositionalVector2 bottomRight, Color color);
        void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color);
        void DrawCircle(PositionalVector2 center, float radius, Color color);
    }
}
