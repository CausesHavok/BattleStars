using System.Numerics;

namespace BattleStars.Utility;

public struct PositionalVector2
{
    private Vector2 _value;

    public Vector2 Value
    {
        get => _value;
        set
        {
            VectorValidator.ThrowIfNaNOrInfinity(value, nameof(value));
            _value = value;
        }
    }

    public PositionalVector2(Vector2 value)
    {
        Value = value;
    }

    public static implicit operator Vector2(PositionalVector2 positionalVector) => positionalVector.Value;
    public static implicit operator PositionalVector2(Vector2 vector) => new(vector);
}