using System.Drawing;
using System.Numerics;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using BattleStars.Core.Guards;

namespace BattleStars.Domain.Entities.Shapes;


internal class Triangle : IShape
{
    public PositionalVector2 Point1 { get; private set; }
    public PositionalVector2 Point2 { get; private set; }
    public PositionalVector2 Point3 { get; private set; }
    public BoundingBox BoundingBox { get; }
    public Color Color { get; private set; }
    private readonly IShapeDrawer _drawer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Triangle"/> class with the specified points.
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the points are relative to the entity's position.
    /// The bounding box is calculated based on the points of the triangle.
    /// </summary>
    /// <param name="point1">The first point of the triangle.</param>
    /// <param name="point2">The second point of the triangle.</param>
    /// <param name="point3">The third point of the triangle.</param>
    public Triangle(PositionalVector2 point1, PositionalVector2 point2, PositionalVector2 point3, Color color, IShapeDrawer drawer)
    {
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;

        if (!IsValidTriangle())
            throw new ArgumentException("The points do not form a valid triangle.");

        Color = color;
        BoundingBox = CalculateBoundingBox();
        _drawer = Guard.NotNull(drawer, nameof(drawer));
    }

    private BoundingBox CalculateBoundingBox()
    {
        var minX = Math.Min(Point1.X, Math.Min(Point2.X, Point3.X));
        var minY = Math.Min(Point1.Y, Math.Min(Point2.Y, Point3.Y));
        var maxX = Math.Max(Point1.X, Math.Max(Point2.X, Point3.X));
        var maxY = Math.Max(Point1.Y, Math.Max(Point2.Y, Point3.Y));
        return new BoundingBox(new PositionalVector2(minX, minY), new PositionalVector2(maxX, maxY));
    }

    private bool IsValidTriangle()
    {
        // Calculate the area using the cross product
        float area = 0.5f * Math.Abs(
            (Point2.X - Point1.X) * (Point3.Y - Point1.Y) -
            (Point3.X - Point1.X) * (Point2.Y - Point1.Y)
        );
        return area > 0;
    }

    public bool Contains(PositionalVector2 point)
    {
        // Check bounding box first for quick rejection;
        if (!BoundingBox.Contains(point)) return false;

        // Vectors from triangle points to the point
        var v0 = Point3 - Point1;
        var v1 = Point2 - Point1;
        var v2 = point - Point1;

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

    public void Draw(PositionalVector2 entityPosition)
    {
        _drawer.DrawTriangle(entityPosition + Point1, entityPosition + Point2, entityPosition + Point3, Color);
    }
}