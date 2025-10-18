using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
using System.Numerics;
namespace BattleStars.Domain.Entities;

public class PlayerMovable : IMovable
{
    private PositionalVector2 _position;
    public PositionalVector2 Position => _position;
    private readonly float _speed;
    private readonly IBoundaryChecker _boundaryChecker;

    public PlayerMovable(PositionalVector2 initialPosition, float speed, IBoundaryChecker boundaryChecker)
    {
        // Removed redundant validation: PositionalVector2 already validates during construction.
        FloatGuard.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatGuard.ThrowIfNegative(speed, nameof(speed));
        FloatGuard.ThrowIfZero(speed, nameof(speed));
        Guard.NotNull(boundaryChecker, nameof(boundaryChecker));

        _position = initialPosition;
        _speed = speed;
        _boundaryChecker = boundaryChecker;
    }

    public void Move(IContext context)
    {
        Guard.NotNull(context, nameof(context));

        var direction = context.PlayerDirection;
        VectorGuard.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (direction == Vector2.Zero)
            return;

        VectorGuard.ThrowIfNotNormalized(direction, nameof(direction));

        var newPosition = _position + direction * _speed;

        if (_boundaryChecker.IsOutsideYBounds(newPosition.Y))
        {
            newPosition.Y -=
                _boundaryChecker.YDistanceToBoundary(newPosition.Y)
                * Math.Sign(direction.Y);
        }
        if (_boundaryChecker.IsOutsideXBounds(newPosition.X))
        {
            newPosition.X -=
                _boundaryChecker.XDistanceToBoundary(newPosition.X)
                * Math.Sign(direction.X);
        }

        _position = newPosition;
    }
}