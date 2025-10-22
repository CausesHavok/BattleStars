using BattleStars.Core.Guards;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Entities;

internal class Shot: IShot
{
    public PositionalVector2 Position { get; private set; }
    public DirectionalVector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Shot(
        PositionalVector2 position,
        DirectionalVector2 direction,
        float speed,
        float damage
    )
    {
        Position = position;
        Direction = direction;
        Speed = speed;
        Damage = damage;
    }

    public static Shot Create(
        PositionalVector2 position,
        DirectionalVector2 direction,
        float speed,
        float damage
    ) 
    => new(
        position,
        direction,
        Guard.RequireNonNegative(speed, nameof(speed)),
        Guard.RequireNonNegative(damage, nameof(damage))
    );

    public void Update()
    {
        if (!IsActive || Speed == 0)
            return;

        Position += Direction * Speed;

    }

    public void Deactivate() => IsActive = false;
}