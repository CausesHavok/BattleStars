using System.Numerics;
using System.Drawing;

namespace BattleStars.Shapes;

public class Polygon : IShape
{
    private readonly IShape[] _shapes;
    public BoundingBox BoundingBox { get; } 

    /// <summary>
    /// Initializes a new instance of the <see cref="Polygon"/> class with the specified triangles
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the triangles are relative to the entity's position.
    /// The bounding box is calculated based on the triangles.
    /// </summary>
    /// <param name="triangles">An array of triangles that make up the polygon.</param>
    /// <exception cref="ArgumentNullException">Thrown when the triangles array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the triangles array has less than three triangles.</exception>
    public Polygon(IShape[] shapes)
    {
        if (shapes == null || shapes.Length == 0)
            throw new ArgumentException("A polygon must have at least one shape.");

        _shapes = shapes;
        BoundingBox = CalculateBoundingBox();
    }

    private BoundingBox CalculateBoundingBox()
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        foreach (var shape in _shapes)
        {
            var box = shape.BoundingBox;
            minX = Math.Min(minX, box.TopLeft.X);
            minY = Math.Min(minY, box.TopLeft.Y);
            maxX = Math.Max(maxX, box.BottomRight.X);
            maxY = Math.Max(maxY, box.BottomRight.Y);
        }

        return new BoundingBox(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }
 

    public bool Contains(Vector2 point, Vector2 entityPosition)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));

        // Check bounding box first for quick rejection
        var adjustedPoint = point - entityPosition;
        if (!BoundingBox.Contains(adjustedPoint)) return false;

        // Check each shape for point containment
        foreach (var shape in _shapes)
        {
            if (shape.Contains(point, entityPosition)) return true;
        }

        return false;
    }

    public void Draw(Vector2 entityPosition, IShapeDrawer drawer)
    {
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));
        ArgumentNullException.ThrowIfNull(drawer);
        foreach (var shape in _shapes)
        {
            shape.Draw(entityPosition, drawer);
        }
    }

}