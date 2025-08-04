using System.Numerics;

namespace BattleStars;

public abstract class Entity
{
    public Vector2 Position { get; protected set; }
    private float _health;
    public float Health
    {
        get => _health;
        protected set => _health = value > 0 ? value : 0;
    }

    public bool IsDead => _health <= 0;

    protected Entity(Vector2 position, float health)
    {
        if (health <= 0) throw new ArgumentOutOfRangeException(nameof(health), "Health must be positive.");
        Position = position;
        Health = health;
    }

    public virtual void Move(Vector2 direction)
    {
        Position += direction;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
        if (IsDead) return; // Ignore further damage if already dead
        Health -= damage;
    }
}