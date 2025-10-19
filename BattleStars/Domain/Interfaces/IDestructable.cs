namespace BattleStars.Domain.Interfaces;
internal interface IDestructable
{
    void TakeDamage(float amount);
    bool IsDestroyed { get; }
}