namespace BattleStars.Core;

public class StandardDestructable(float initialHealth) : IDestructable
{
    private float _health = initialHealth;

    public float Health
    {
        get => _health;
        private set => _health = value > 0 ? value : 0;
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }
}
