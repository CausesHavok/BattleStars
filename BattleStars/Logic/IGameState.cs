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
    IContext Context { get; }
    IBattleStar Player { get; }
    List<IBattleStar> Enemies { get; }
    List<IShot> PlayerShots { get; }
    List<IShot> EnemyShots { get; }
}