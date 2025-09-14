using System.Numerics;
using BattleStars.Shots;
using BattleStars.Utility;

namespace BattleStars.Core;

/// <summary>
/// A basic implementation of <see cref="IShooter"/> that shoots a single shot in a specified direction.
/// </summary>
/// <remarks>
/// This class is intended for decoration with more complex shooters.
/// </remarks>
public class BasicShooter : IShooter
{
    private readonly Func<Vector2, Vector2, IShot> _shotFactory;
    private readonly Vector2 _direction;

    public BasicShooter(Func<Vector2, Vector2, IShot> shotFactory, Vector2 direction)
    {
        ArgumentNullException.ThrowIfNull(shotFactory, nameof(shotFactory));
        VectorValidator.ThrowIfNaNOrInfinity(direction, nameof(direction));
        if (direction != Vector2.Zero) VectorValidator.ThrowIfNotNormalized(direction, nameof(direction));

        _shotFactory = shotFactory;
        _direction = direction;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation creates a single shot using the provided shot factory and the specified direction.
    /// </remarks>
    public IEnumerable<IShot> Shoot(IContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var shot = _shotFactory(context.ShooterPosition, _direction);
        return [shot];
    }
}