using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Shots;

public class Shot : IShot
{
    public PositionalVector2 Position { get; private set; }
    public DirectionalVector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Shot(PositionalVector2 position, DirectionalVector2 direction, float speed, float damage)
    {
        FloatValidator.ThrowIfNegative(speed, nameof(speed));
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNegative(damage, nameof(damage));
        FloatValidator.ThrowIfNaNOrInfinity(damage, nameof(damage));

        Position = position;
        Direction = direction;
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