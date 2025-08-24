using Raylib_cs;
using System.Numerics;
namespace BattleStars.Shapes;

public class RaylibShapeDrawer : IShapeDrawer
{

    private static Color ToRaylibColor(System.Drawing.Color color)
        => new(color.R, color.G, color.B, color.A);
    public void DrawRectangle(Vector2 v1, Vector2 v2, System.Drawing.Color color)
    {
        Raylib.DrawRectangle((int)v1.X, (int)v1.Y, (int)(v2.X - v1.X), (int)(v2.Y - v1.Y), ToRaylibColor(color));
    }

    public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, System.Drawing.Color color)
    {
        Raylib.DrawTriangle(p1, p2, p3, ToRaylibColor(color));
    }

    public void DrawCircle(Vector2 center, float radius, System.Drawing.Color color)
    {
        Raylib.DrawCircle((int)center.X, (int)center.Y, radius, ToRaylibColor(color));
    }

}