using System.Drawing;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using BattleStars.Infrastructure.Utilities;

namespace BattleStars.Domain.Entities.Shapes;

public class Circle : IShape
{
    private readonly float _radius;
    private readonly Color _color;
    public BoundingBox BoundingBox { get; }
    private readonly IShapeDrawer _drawer;

    public Circle(float radius, Color color, IShapeDrawer drawer)
    {
        ArgumentNullException.ThrowIfNull(drawer);
        FloatValidator.ThrowIfNaNOrInfinity(radius, nameof(radius));
        FloatValidator.ThrowIfNegative(radius, nameof(radius));
        FloatValidator.ThrowIfZero(radius, nameof(radius));

        _radius = radius;
        _color = color;
        BoundingBox = new BoundingBox(new PositionalVector2(-_radius, -_radius), new PositionalVector2(_radius, _radius));
        _drawer = drawer;
    }

    public bool Contains(PositionalVector2 point)
    {

        return point.Position.LengthSquared() <= _radius * _radius;
    }

    public void Draw(PositionalVector2 position)
    {
        _drawer.DrawCircle(position, _radius, _color);
    }
}
