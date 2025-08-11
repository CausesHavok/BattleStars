using System.Numerics;
using System.Drawing;

namespace BattleStars.Shapes;

public class Rectangle : IShape
{
    public Vector2 TopLeft { get; private set; }
    public Vector2 BottomRight { get; private set; }
    public Color Color { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class with the specified top-left and bottom-right corners.
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the top-left and bottom-right points are relative to the entity's position.
    /// The bounding box is set to the rectangle itself.
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    public Rectangle(Vector2 v1, Vector2 v2, Color color)
    {
        VectorValidator.ThrowIfNaNOrInfinity(v1, nameof(v1));
        VectorValidator.ThrowIfNaNOrInfinity(v2, nameof(v2));

        float minX = Math.Min(v1.X, v2.X);
        float minY = Math.Min(v1.Y, v2.Y);
        float maxX = Math.Max(v1.X, v2.X);
        float maxY = Math.Max(v1.Y, v2.Y);

        if (minX == maxX || minY == maxY)
        throw new ArgumentException("Rectangle must have non-zero width and height.");

        TopLeft = new Vector2(minX, minY);
        BottomRight = new Vector2(maxX, maxY);
        Color = color;
    }

    public bool Contains(Vector2 point, Vector2 entityPosition)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));
        // Adjust the point based on the shape's offset
        // Point within(offset + triangle) <=> (point - offset) within triangle
        // This allows us to reduce save on calculations by not having to adjust the triangle points
        var adjustedPoint = point - entityPosition;
        return adjustedPoint.X >= TopLeft.X && adjustedPoint.X <= BottomRight.X &&
               adjustedPoint.Y >= TopLeft.Y && adjustedPoint.Y <= BottomRight.Y;
    }

    public void Draw(Vector2 position, IShapeDrawer drawer)
    {
        VectorValidator.ThrowIfNaNOrInfinity(position, nameof(position));
        ArgumentNullException.ThrowIfNull(drawer);
        drawer.DrawRectangle(position + TopLeft, position + BottomRight, Color);
    }
}