using System.Drawing;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;
using BattleStars.Core.Guards;

namespace BattleStars.Domain.Entities.Shapes;

internal class Circle : IShape
{
    private readonly float _radius;
    private readonly Color _color;
    public BoundingBox BoundingBox { get; }
    private readonly IShapeDrawer _drawer;

    public Circle(float radius, Color color, IShapeDrawer drawer)
    {
        _radius = Guard.RequirePositive(radius);
        _color = color;
        BoundingBox = new BoundingBox(new PositionalVector2(-_radius, -_radius), new PositionalVector2(_radius, _radius));
        _drawer = Guard.NotNull(drawer);
    }

    public bool Contains(PositionalVector2 point) => point.Position.LengthSquared() <= _radius * _radius;

    public void Draw(PositionalVector2 position) => _drawer.DrawCircle(position, _radius, _color);
}
