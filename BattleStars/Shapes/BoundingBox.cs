using System.Numerics;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public readonly struct BoundingBox(PositionalVector2 topLeft, PositionalVector2 bottomRight) :
    IContains<PositionalVector2>
{
    public PositionalVector2 TopLeft { get; } = topLeft;
    public PositionalVector2 BottomRight { get; } = bottomRight;

    public bool Contains(PositionalVector2 point)
    {
        return point.X >= TopLeft.X && point.X <= BottomRight.X &&
               point.Y >= TopLeft.Y && point.Y <= BottomRight.Y;
    }
}