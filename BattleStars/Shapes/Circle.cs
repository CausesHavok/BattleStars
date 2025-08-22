using System.Numerics;
using System.Drawing;

namespace BattleStars.Shapes;

public class Circle : IShape
{
    private readonly float _radius;
    private readonly Color _color;
    public BoundingBox BoundingBox { get; }

    public Circle(float radius, Color color)
    {
        FloatValidator.ThrowIfNaNOrInfinity(radius, nameof(radius));
        FloatValidator.ThrowIfNegative(radius, nameof(radius));
        FloatValidator.ThrowIfZero(radius, nameof(radius));

        _radius = radius;
        _color = color;
        BoundingBox = new BoundingBox(new Vector2(-_radius, -_radius), new Vector2(_radius, _radius));
    }

    public bool Contains(Vector2 point, Vector2 entityPosition)
    {
        VectorValidator.ThrowIfNaNOrInfinity(point, nameof(point));
        VectorValidator.ThrowIfNaNOrInfinity(entityPosition, nameof(entityPosition));

        // Adjust the point based on the shape's offset
        point -= entityPosition;
        return point.LengthSquared() <= _radius * _radius;
    }

    public void Draw(Vector2 position, IShapeDrawer drawer)
    {
        VectorValidator.ThrowIfNaNOrInfinity(position, nameof(position));
        FloatValidator.ThrowIfNaNOrInfinity(_radius, nameof(_radius));
        ArgumentNullException.ThrowIfNull(drawer);

        drawer.DrawCircle(position, _radius, _color);
    }
}
