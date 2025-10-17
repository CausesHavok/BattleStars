using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities;

public class BattleStar : IBattleStar
{
    private readonly IShape _shape;
    private readonly IMovable _movable;
    private readonly IDestructable _destructable;
    private readonly IShooter _shooter;

    public BattleStar(IShape shape, IMovable movable, IDestructable destructable, IShooter shooter)
    {
        Guard.NotNull(shape, nameof(shape));
        Guard.NotNull(movable, nameof(movable));
        Guard.NotNull(destructable, nameof(destructable));
        Guard.NotNull(shooter, nameof(shooter));

        _shape = shape;
        _movable = movable;
        _destructable = destructable;
        _shooter = shooter;
    }

    public void Draw() => _shape.Draw(_movable.Position);

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