using System.Numerics;
using BattleStars.Shapes;
using BattleStars.Utility;
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

    private readonly Func<Vector2, Vector2, IShot> _shotFactory;

    public bool IsDead => _health <= 0;

    public IShape Shape { get; private set; }

    protected Entity(Vector2 position, float health, Func<Vector2, Vector2, IShot> shotFactory, IShape shape)
    {
        _shotFactory = shotFactory ?? throw new ArgumentNullException(nameof(shotFactory), "Shot factory cannot be null.");
        Shape = shape ?? throw new ArgumentNullException(nameof(shape), "Shape cannot be null.");
        FloatValidator.ThrowIfNaNOrInfinity(health, nameof(health));
        FloatValidator.ThrowIfNegative(health, nameof(health));
        FloatValidator.ThrowIfZero(health, nameof(health));
        VectorValidator.ThrowIfNaNOrInfinity(position, nameof(position));
        Position = position;
        Health = health;
    }

    public virtual void Move(Vector2 direction)
    {
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (IsDead) return;
        if (direction == Vector2.Zero) return;
        Position += direction;
    }

    public void TakeDamage(float damage)
    {
        FloatValidator.ThrowIfNaNOrInfinity(damage, nameof(damage));
        FloatValidator.ThrowIfNegative(damage, nameof(damage));
        if (IsDead) return;
        Health -= damage;
    }

    public IShot Shoot(Vector2 direction)
    {
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));
        if (IsDead) throw new InvalidOperationException("Cannot shoot when dead.");
        return _shotFactory(Position, direction);
    }
}