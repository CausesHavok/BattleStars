using System.Numerics;

namespace BattleStars;

public class Player
{
    public Vector2 Position { get; private set; }
    private float _health;
    public float Health
    {
        get => _health;
        private set => _health = value > 0 ? value : 0;
    }

    public bool IsDead => _health <= 0;

    private readonly IBoundaryChecker boundaryChecker;

    public Player(Vector2 position, float health, IBoundaryChecker boundaryChecker)
    {
        if (health <= 0) throw new ArgumentOutOfRangeException(nameof(health), "Health must be positive.");
        Position = position;
        Health = health;
        this.boundaryChecker = boundaryChecker ?? throw new ArgumentNullException(nameof(boundaryChecker), "Boundary checker cannot be null.");
    }


    public void Move(Vector2 direction)
    {
        if (boundaryChecker.IsOutsideXBounds(Position.X + direction.X))
        {
            direction.X = 0;
        }
        if (boundaryChecker.IsOutsideYBounds(Position.Y + direction.Y))
        {
            direction.Y = 0;
        }
        Position += direction;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
        if (IsDead) return; // Ignore further damage if already dead
        Health -= damage;
    }


}
