using System.Drawing;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using BattleStars.Core.Guards;

namespace BattleStars.Domain.Entities.Shapes;

internal class Rectangle : IShape
{
    public Color Color { get; private set; }
    public BoundingBox BoundingBox { get; }
    private readonly IShapeDrawer _drawer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class with the specified top-left and bottom-right corners.
    /// It is assumed that the shape is created around the origin (0, 0), as the entities that use this class use their position as an offset for the shape.
    /// This means that the top-left and bottom-right points are relative to the entity's position.
    /// The bounding box is set to the rectangle itself.
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    public Rectangle(PositionalVector2 v1, PositionalVector2 v2, Color color, IShapeDrawer drawer)
    {
        Guard.NotNull(drawer, nameof(drawer));
        float minX = Math.Min(v1.X, v2.X);
        float minY = Math.Min(v1.Y, v2.Y);
        float maxX = Math.Max(v1.X, v2.X);
        float maxY = Math.Max(v1.Y, v2.Y);

        if (minX == maxX || minY == maxY)
            throw new ArgumentException("Rectangle must have non-zero width and height.");

        BoundingBox = new BoundingBox(new PositionalVector2(minX, minY), new PositionalVector2(maxX, maxY));
        Color = color;
        _drawer = drawer;
    }

    public bool Contains(PositionalVector2 point)
    {
        return BoundingBox.Contains(point);
    }

    public void Draw(PositionalVector2 position)
    {
        _drawer.DrawRectangle(position + BoundingBox.TopLeft, position + BoundingBox.BottomRight, Color);
    }
}