using System.Numerics;

namespace BattleStars.Utility;

public struct DirectionalVector2
{
    private Vector2 _direction;

    public readonly float X => _direction.X;
    public readonly float Y => _direction.Y;

    public Vector2 Direction
    {
        get => _direction;
        set
        {
            VectorValidator.ThrowIfNaNOrInfinity(value, nameof(Direction));
            if (value != Vector2.Zero)
            {
                VectorValidator.ThrowIfNotNormalized(value, nameof(Direction));
            }
            _direction = value;
        }
    }

    public DirectionalVector2(Vector2 direction)
    {
        Direction = direction;
    }

    public DirectionalVector2(float x, float y) : this(new Vector2(x, y))
    {
    }

    public static implicit operator Vector2(DirectionalVector2 directionalVector) => directionalVector.Direction;
    public static implicit operator DirectionalVector2(Vector2 vector) => new(vector);
    
    public static Vector2 operator *(DirectionalVector2 directional, float scalar)
    {
        return directional.Direction * scalar;
    }
}