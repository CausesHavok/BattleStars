using System.Numerics;
using System.Drawing;

namespace BattleStars.Shapes;

public class Triangle : IShape
{
    public Vector2 _point1 { get; private set; }
    public Vector2 _point2 { get; private set; }
    public Vector2 _point3 { get; private set; }
    public Rectangle BoundingBox { get; private set; }

    public Color Color { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Triangle"/> class with the specified points.
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the points are relative to the entity's position.
    /// The bounding box is calculated based on the points of the triangle.
    /// </summary>
    /// <param name="point1">The first point of the triangle.</param>
    /// <param name="point2">The second point of the triangle.</param>
    /// <param name="point3">The third point of the triangle.</param>
    public Triangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point1, nameof(point1));
        VectorValidator.ThrowIfNaNOrInfinity(point2, nameof(point2));
        VectorValidator.ThrowIfNaNOrInfinity(point3, nameof(point3));

        _point1 = point1;
        _point2 = point2;
        _point3 = point3;

        if (!IsValidTriangle())
            throw new ArgumentException("The points do not form a valid triangle.");

        Color = color;
        BoundingBox = CalculateBoundingBox();
    }

    private Rectangle CalculateBoundingBox()
    {
        var minX = Math.Min(_point1.X, Math.Min(_point2.X, _point3.X));
        var minY = Math.Min(_point1.Y, Math.Min(_point2.Y, _point3.Y));
        var maxX = Math.Max(_point1.X, Math.Max(_point2.X, _point3.X));
        var maxY = Math.Max(_point1.Y, Math.Max(_point2.Y, _point3.Y));
        return new Rectangle(new Vector2(minX, minY), new Vector2(maxX, maxY), Color);
    }

    private bool IsValidTriangle()
    {
        // Calculate the area using the cross product
        float area = 0.5f * Math.Abs(
            (_point2.X - _point1.X) * (_point3.Y - _point1.Y) -
            (_point3.X - _point1.X) * (_point2.Y - _point1.Y)
        );
        return area > 0;
    }

    public bool Contains(Vector2 point, Vector2 entityPosition)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));
        // Check bounding box first for quick rejection
        if (!BoundingBox.Contains(point, entityPosition)) return false;

        // Adjust the point based on the shape's offset
        var adjustedPoint = point - entityPosition;

        // Vectors from triangle points to the point
        var v0 = _point3 - _point1;
        var v1 = _point2 - _point1;
        var v2 = adjustedPoint - _point1;

        // Compute dot products
        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);

        // Compute barycentric coordinates
        float denom = dot00 * dot11 - dot01 * dot01;
        if (denom == 0) return false; // Degenerate triangle

        float u = (dot11 * dot02 - dot01 * dot12) / denom;
        float v = (dot00 * dot12 - dot01 * dot02) / denom;

        // Check if point is in triangle
        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }

    public void Draw(Vector2 entityPosition, IShapeDrawer drawer)
    {
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));
        ArgumentNullException.ThrowIfNull(drawer);
        drawer.DrawTriangle(entityPosition + _point1, entityPosition + _point2, entityPosition + _point3, Color);
    }
}