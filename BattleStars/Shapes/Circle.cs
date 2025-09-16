using System.Numerics;
using System.Drawing;
using BattleStars.Utility;
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
        BoundingBox = new BoundingBox(new PositionalVector2(-_radius, -_radius), new PositionalVector2(_radius, _radius));
    }

    public bool Contains(PositionalVector2 point)
    {

        return point.Position.LengthSquared() <= _radius * _radius;
    }

    public void Draw(PositionalVector2 position, IShapeDrawer drawer)
    {
        ArgumentNullException.ThrowIfNull(drawer);

        drawer.DrawCircle(position, _radius, _color);
    }
}
