using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public class PlayerMovable : IMovable
{
    private Vector2 _position;
    public Vector2 Position => _position;
    private readonly float _speed;

    public PlayerMovable(Vector2 initialPosition, float speed)
    {
        VectorValidator.ThrowIfNaNOrInfinity(initialPosition, nameof(initialPosition));
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNegative(speed, nameof(speed));
        FloatValidator.ThrowIfZero(speed, nameof(speed));
        
        _position = initialPosition;
        _speed = speed;
    }

    public void Move(IContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var direction = context.PlayerDirection;
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));

        _position += direction * _speed;
    }
}