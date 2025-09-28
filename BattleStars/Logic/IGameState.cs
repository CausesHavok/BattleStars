using BattleStars.Core;
using BattleStars.Shots;
namespace BattleStars.Logic;

/// <summary>
/// Represents the current state of the game, including context, player, enemies, and shots.
/// </summary>
/// <remarks>
/// This interface defines the structure for managing the game's state at any given moment.
/// </remarks>
public interface IGameState
{
    IContext Context { get; set; }
    IBattleStar Player { get; set; }
    List<IBattleStar> Enemies { get; set; }
    List<IShot> PlayerShots { get; set; }
    List<IShot> EnemyShots { get; set; }

    /// <summary>
    /// Validates the internal consistency of the game state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if any validation checks fail.</exception>
    /// <remarks>
    public void Validate();

}