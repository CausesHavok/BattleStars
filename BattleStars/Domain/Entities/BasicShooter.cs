using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities;

/// <summary>
/// A basic implementation of <see cref="IShooter"/> that shoots a single shot in a specified direction.
/// </summary>
/// <remarks>
/// This class is intended for decoration with more complex shooters.
/// </remarks>
internal class BasicShooter : IShooter
{
    private readonly Func<PositionalVector2, DirectionalVector2, IShot> _shotFactory;
    private readonly DirectionalVector2 _direction;

    public BasicShooter(Func<PositionalVector2, DirectionalVector2, IShot> shotFactory, DirectionalVector2 direction)
    {
        Guard.NotNull(shotFactory, nameof(shotFactory));

        _shotFactory = shotFactory;
        _direction = direction;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation creates a single shot using the provided shot factory and the specified direction.
    /// </remarks>
    public IEnumerable<IShot> Shoot(IContext context)
    {
        Guard.NotNull(context, nameof(context));

        var shot = _shotFactory(context.ShooterPosition, _direction);
        return [shot];
    }
}