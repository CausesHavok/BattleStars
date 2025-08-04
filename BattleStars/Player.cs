using System.Numerics;

namespace BattleStars;

public class Player
{
    public Vector2 Position { get; private set; }
    public float Health { get; private set; } = 100f; // Default health value
    public bool IsDead => Health <= 0;

    private readonly IBoundaryChecker boundaryChecker;

    public Player(Vector2 position, float health, IBoundaryChecker boundaryChecker)
    {
        Position = position;
        Health = health;
        this.boundaryChecker = boundaryChecker;
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

    public void TakeHit(float damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
        // Optionally: trigger death logic here if Health == 0
    }


}
