using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

public interface IShot
{
    PositionalVector2 Position { get; }
    DirectionalVector2 Direction { get; }
    float Speed { get; }
    float Damage { get; }
    bool IsActive { get; }
    void Update();
}