using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public class BasicMovable : IMovable
{
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        private set
        {
            VectorValidator.ThrowIfNaNOrInfinity(value, nameof(value));
            _position = value;
        }
    }

    private readonly Vector2 _direction;
    private readonly float _speed;

    public BasicMovable(Vector2 initialPosition, Vector2 direction, float speed)
    {
        VectorValidator.ThrowIfNaNOrInfinity(initialPosition, nameof(initialPosition));
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (direction != Vector2.Zero) VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNegativeOrZero(speed, nameof(speed));

        Position = initialPosition;
        _direction = direction;
        _speed = speed;
    }

    public void Move(IContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        Position += _direction * _speed;
    }
}