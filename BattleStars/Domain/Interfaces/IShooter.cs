namespace BattleStars.Domain.Interfaces;

internal interface IShooter
{
    /// <summary>
    /// Defines a shooter capable of firing any number of shots
    /// </summary>
    /// <param name="context">
    /// The context for the shot.
    /// This method assumes that Context.ShooterPosition is not NaN/Infinity and is a valid Vector2.
    /// </param>
    /// <returns>
    /// An enumerable collection of shots fired by the shooter.
    /// </returns>
    IEnumerable<IShot> Shoot(IContext context);
}