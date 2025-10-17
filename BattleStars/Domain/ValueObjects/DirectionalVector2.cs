using System.Numerics;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.ValueObjects;

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
    // Implicit conversion from Vector2 to DirectionalVector2 is intentionally disabled to avoid accidental creation of non-normalized directional vectors.

    public static Vector2 operator *(DirectionalVector2 directional, float scalar)
    {
        return directional.Direction * scalar;
    }
    public static DirectionalVector2 operator -(DirectionalVector2 v)
    {
        return new DirectionalVector2(-v.X, -v.Y);
    }
    public static readonly DirectionalVector2 Zero = new(Vector2.Zero);
    public static readonly DirectionalVector2 UnitX = new(Vector2.UnitX);
    public static readonly DirectionalVector2 UnitY = new(Vector2.UnitY);
}