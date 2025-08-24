using System.Numerics;
using System.Drawing;
namespace BattleStars.Shapes;

public interface IShapeDrawer
{
    void DrawRectangle(Vector2 v1, Vector2 v2, Color color);
    void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color);
    void DrawCircle(Vector2 center, float radius, Color color);
}