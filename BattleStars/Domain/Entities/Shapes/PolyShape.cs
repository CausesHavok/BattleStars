using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Domain.Entities.Shapes;

internal class PolyShape : IShape
{
    private readonly IShape[] _shapes;
    public BoundingBox BoundingBox { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolyShape"/> class with the specified triangles
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the triangles are relative to the entity's position.
    /// The bounding box is calculated based on the triangles.
    /// </summary>
    /// <param name="triangles">An array of triangles that make up the polygon.</param>
    /// <exception cref="ArgumentNullException">Thrown when the triangles array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the triangles array has less than three triangles.</exception>
    public PolyShape(IShape[] shapes)
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

        return new BoundingBox(new PositionalVector2(minX, minY), new PositionalVector2(maxX, maxY));
    }
 

    public bool Contains(PositionalVector2 point)
    {
        // Check bounding box first for quick rejection
        if (!BoundingBox.Contains(point)) return false;

        // Check each shape for point containment
        foreach (var shape in _shapes)
        {
            if (shape.Contains(point)) return true;
        }

        return false;
    }

    public void Draw(PositionalVector2 entityPosition)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(entityPosition);
        }
    }

}