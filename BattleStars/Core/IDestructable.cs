namespace BattleStars.Core;

public interface IDestructable
{
    void TakeDamage(float amount);
    bool IsDestroyed { get; }
}