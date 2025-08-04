using System.Numerics;

namespace BattleStars;

public class Player : Entity
{
    private readonly IBoundaryChecker boundaryChecker;

    public Player(Vector2 position, float health, IBoundaryChecker boundaryChecker) : base(position, health)
    {
        this.boundaryChecker = boundaryChecker ?? throw new ArgumentNullException(nameof(boundaryChecker), "Boundary checker cannot be null.");
    }

    public override void Move(Vector2 direction)
    {
        if (boundaryChecker.IsOutsideXBounds(Position.X + direction.X))
        {
            direction.X = 0;
        }
        if (boundaryChecker.IsOutsideYBounds(Position.Y + direction.Y))
        {
            direction.Y = 0;
        }
        base.Move(direction);
    }
}