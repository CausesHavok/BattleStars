namespace BattleStars.Core;

public interface IDestructable
{
    void TakeDamage(float amount);
    bool IsDead();
}