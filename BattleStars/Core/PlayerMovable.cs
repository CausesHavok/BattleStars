using System.Numerics;
using BattleStars.Logic;
using BattleStars.Utility;

namespace BattleStars.Core;

public class PlayerMovable : IMovable
{
    private Vector2 _position;
    public Vector2 Position => _position;
    private readonly float _speed;
    private readonly IBoundaryChecker _boundaryChecker;

    public PlayerMovable(Vector2 initialPosition, float speed, IBoundaryChecker boundaryChecker)
    {
        VectorValidator.ThrowIfNaNOrInfinity(initialPosition, nameof(initialPosition));
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNegative(speed, nameof(speed));
        FloatValidator.ThrowIfZero(speed, nameof(speed));
        ArgumentNullException.ThrowIfNull(boundaryChecker, nameof(boundaryChecker));

        _position = initialPosition;
        _speed = speed;
        _boundaryChecker = boundaryChecker;
    }

    public void Move(IContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var direction = context.PlayerDirection;
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (direction == Vector2.Zero)
            return;
            
        VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));

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