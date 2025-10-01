using Raylib_cs;
using BattleStars.Domain.ValueObjects;
namespace BattleStars.Infrastructure.Adapters;

/// <summary>
/// Adapter class to bridge between BattleStars' IShapeDrawer and Raylib's drawing functions.
/// </summary>
/// <remarks>
/// This adapter is intentionally thin and delegates directly to Raylib.
/// It exists to enable testability of higher-level logic via IRaylibGraphics.
/// Direct testing is not applicable; excluded from coverage.
/// </remarks>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

