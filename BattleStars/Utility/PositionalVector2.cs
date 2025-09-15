using System.Numerics;

namespace BattleStars.Utility;

public struct PositionalVector2
{
    private Vector2 _position;

    public readonly float X => _position.X;
    public readonly float Y => _position.Y;

    public Vector2 Position
    {
        get => _position;
        set
        {
            VectorValidator.ThrowIfNaNOrInfinity(value, nameof(Position));
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
    public static implicit operator PositionalVector2(Vector2 vector) => new(vector);
}