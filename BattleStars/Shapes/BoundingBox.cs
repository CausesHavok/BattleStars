using System.Numerics;
namespace BattleStars.Shapes;

public readonly struct BoundingBox(Vector2 topLeft, Vector2 bottomRight) :
    IContains<Vector2>
{
    public Vector2 TopLeft { get; } = topLeft;
    public Vector2 BottomRight { get; } = bottomRight;

    public bool Contains(Vector2 point)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        return point.X >= TopLeft.X && point.X <= BottomRight.X &&
               point.Y >= TopLeft.Y && point.Y <= BottomRight.Y;
    }
}