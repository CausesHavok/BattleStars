using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Entities;

public class Shot(
    PositionalVector2 position,
    DirectionalVector2 direction,
    float speed,
    float damage
    ) : IShot
{
    public PositionalVector2 Position { get; private set; } = position;
    public DirectionalVector2 Direction { get; private set; } = direction;
    public float Speed { get; private set; } = speed;
    public float Damage { get; private set; } = damage;
    public bool IsActive { get; private set; } = true;

    public void Update()
    {
        if (!IsActive || Speed == 0)
            return;

        Position += Direction * Speed;

    }

    public void Deactivate() => IsActive = false;
}