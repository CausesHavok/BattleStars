using System.Numerics;
using BattleStars.Utility;

namespace BattleStars;

public class Shot : IShot
{
    public Vector2 Position { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Shot(Vector2 position, Vector2 direction, float speed, float damage)
    {
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));
        VectorValidator.ThrowIfNaNOrInfinity(position, nameof(position));
        FloatValidator.ThrowIfNegative(speed, nameof(speed));
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNegative(damage, nameof(damage));
        FloatValidator.ThrowIfNaNOrInfinity(damage, nameof(damage));

        Position = position;
        Direction = Vector2.Normalize(direction);
        Speed = speed;
        Damage = damage;
    }

    public void Update()
    {
        if (!IsActive)
            return;

        if (Speed == 0)
            return;

        Position += Direction * Speed;

    }

    public void Deactivate()
    {
        IsActive = false;
    }
}