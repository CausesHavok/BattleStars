using System.Numerics;
using System.Drawing;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public class Rectangle : IShape
{
    public Color Color { get; private set; }

    public BoundingBox BoundingBox { get; }

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

        BoundingBox = new BoundingBox(new Vector2(minX, minY), new Vector2(maxX, maxY));
        Color = color;
    }

    public bool Contains(Vector2 point)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        return BoundingBox.Contains(point);
    }

    public void Draw(Vector2 position, IShapeDrawer drawer)
    {
        VectorValidator.ThrowIfNaNOrInfinity(position, nameof(position));
        ArgumentNullException.ThrowIfNull(drawer);
        drawer.DrawRectangle(position + BoundingBox.TopLeft, position + BoundingBox.BottomRight, Color);
    }
}