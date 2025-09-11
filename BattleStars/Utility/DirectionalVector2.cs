using System.Numerics;

namespace BattleStars.Utility;

public struct DirectionalVector2
{
    private Vector2 _value;

    public Vector2 Value
    {
        get => _value;
        set
        {
            VectorValidator.ThrowIfNaNOrInfinity(value, nameof(value));
            if (value != Vector2.Zero)
            {
                VectorValidator.ThrowIfNotNormalized(value, nameof(value)); 
            } 
            _value = value;
        }
    }

    public DirectionalVector2(Vector2 value)
    {
        Value = value;
    }

    public static implicit operator Vector2(DirectionalVector2 directionalVector) => directionalVector.Value;
    public static implicit operator DirectionalVector2(Vector2 vector) => new(vector);
}