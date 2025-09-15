using System.Numerics;
using System.Drawing;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public interface IShapeDrawer
{
    void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color);
    void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color);
    void DrawCircle(PositionalVector2 center, float radius, Color color);
}