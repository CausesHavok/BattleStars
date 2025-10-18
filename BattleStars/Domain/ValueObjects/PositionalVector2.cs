using System.Numerics;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.ValueObjects;

public struct PositionalVector2
{
    private Vector2 _position;

    public float X
    {
        get => _position.X;
        set
        {
            FloatGuard.ThrowIfNaNOrInfinity(value, nameof(X));
            _position.X = value;
        }
    }
    public float Y
    {
        get => _position.Y;
        set
        {
            FloatGuard.ThrowIfNaNOrInfinity(value, nameof(Y));
            _position.Y = value;
        }
    }

    public Vector2 Position
    {
        get => _position;
        set
        {
            VectorGuard.ThrowIfNaNOrInfinity(value, nameof(Position));
            _position = value;
        }
    }

    public PositionalVector2(Vector2 position)
    {
        Position = position;
    }

    public PositionalVector2(float x, float y) : this(new Vector2(x, y))
    {
    }

    public static implicit operator Vector2(PositionalVector2 positionalVector) => positionalVector.Position;

    public static PositionalVector2 operator +(PositionalVector2 positional, Vector2 vector)
    {
        return new PositionalVector2(positional.Position + vector);
    }
    public static PositionalVector2 operator -(PositionalVector2 positional, Vector2 vector)
    {
        return new PositionalVector2(positional.Position - vector);
    }
    public static PositionalVector2 operator -(PositionalVector2 v)
    {
        return new PositionalVector2(-v.X, -v.Y);
    }

    public static readonly PositionalVector2 Zero = new(Vector2.Zero);
    public static readonly PositionalVector2 UnitX = new(Vector2.UnitX);
    public static readonly PositionalVector2 UnitY = new(Vector2.UnitY);
}