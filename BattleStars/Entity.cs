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
        if (float.IsNaN(health)) throw new ArgumentException("Health cannot be NaN.", nameof(health));
        if (float.IsInfinity(health)) throw new ArgumentException("Health cannot be Infinity.", nameof(health));
        if (health <= 0) throw new ArgumentOutOfRangeException(nameof(health), "Health must be positive.");
        Position = position;
        Health = health;
    }

    public virtual void Move(Vector2 direction)
    {
        if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
            throw new ArgumentException("Movement vector contains NaN.", nameof(direction));
        if (float.IsInfinity(direction.X) || float.IsInfinity(direction.Y))
            throw new ArgumentException("Movement vector contains Infinity.", nameof(direction));
        if (IsDead) return;
        if (direction == Vector2.Zero) return;
        Position += direction;
    }

    public void TakeDamage(float damage)
    {
        if (float.IsNaN(damage)) throw new ArgumentException("Damage cannot be NaN.", nameof(damage));
        if (float.IsInfinity(damage)) throw new ArgumentException("Damage cannot be Infinity.", nameof(damage));
        if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
        if (IsDead) return; // Ignore further damage if already dead
        Health -= damage;
    }
}