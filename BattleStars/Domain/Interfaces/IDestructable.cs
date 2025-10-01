namespace BattleStars.Domain.Interfaces;
public interface IDestructable
{
    void TakeDamage(float amount);
    bool IsDestroyed { get; }
}