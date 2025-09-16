using BattleStars.Shots;
using BattleStars.Shapes;
using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public class BattleStar : IBattleStar
{
    private readonly IShape _shape;
    private readonly IShapeDrawer _shapeDrawer;
    private readonly IMovable _movable;
    private readonly IDestructable _destructable;
    private readonly IShooter _shooter;

    public BattleStar(IShape shape, IShapeDrawer shapeDrawer, IMovable movable, IDestructable destructable, IShooter shooter)
    {
        ArgumentNullException.ThrowIfNull(shape, nameof(shape));
        ArgumentNullException.ThrowIfNull(shapeDrawer, nameof(shapeDrawer));
        ArgumentNullException.ThrowIfNull(movable, nameof(movable));
        ArgumentNullException.ThrowIfNull(destructable, nameof(destructable));
        ArgumentNullException.ThrowIfNull(shooter, nameof(shooter));

        _shape = shape;
        _shapeDrawer = shapeDrawer;
        _movable = movable;
        _destructable = destructable;
        _shooter = shooter;
    }

    public void Draw() => _shape.Draw(_movable.Position, _shapeDrawer);

    public BoundingBox GetBoundingBox() => _shape.BoundingBox;

    public bool Contains(PositionalVector2 point)
    {
        var adjustedPoint = point - _movable.Position;
        return _shape.Contains(adjustedPoint);
    }

    public void Move(IContext context) => _movable.Move(context);

    public void TakeDamage(float amount) => _destructable.TakeDamage(amount);

    public bool IsDestroyed => _destructable.IsDestroyed;

    public IEnumerable<IShot> Shoot(IContext context)
    {
        context.ShooterPosition = _movable.Position;
        return _shooter.Shoot(context);
    }
}