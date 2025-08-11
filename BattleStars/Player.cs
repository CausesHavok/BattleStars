using System.Numerics;
using BattleStars.Shapes;

namespace BattleStars;

public class Player : Entity
{
    private readonly IBoundaryChecker boundaryChecker;

    public Player(
        Vector2 position,
        float health,
        IBoundaryChecker boundaryChecker,
        Func<Vector2, Vector2, IShot> shotFactory,
        IShape shape) : base(position, health, shotFactory, shape)
    {
        this.boundaryChecker = boundaryChecker ?? throw new ArgumentNullException(nameof(boundaryChecker), "Boundary checker cannot be null.");
    }

    public override void Move(Vector2 direction)
    {
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (IsDead) return;
        
        if (boundaryChecker.IsOutsideXBounds(Position.X + direction.X))
        {
            float distX = boundaryChecker.XDistanceToBoundary(Position.X + direction.X);
            direction.X -= distX * Math.Sign(direction.X);
        }
        if (boundaryChecker.IsOutsideYBounds(Position.Y + direction.Y))
        {
            float distY = boundaryChecker.YDistanceToBoundary(Position.Y + direction.Y);
            direction.Y -= distY * Math.Sign(direction.Y);
        }
        base.Move(direction);
    }
}