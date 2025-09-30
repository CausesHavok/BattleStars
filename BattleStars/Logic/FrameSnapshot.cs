using BattleStars.Core;
using BattleStars.Shots;

namespace BattleStars.Logic;

/// <summary>
/// Frame snapshot containing the state of the game at a specific frame.
/// </summary>
/// <param name="Player">The player character.</param>
/// <param name="Enemies">The enemy characters.</param>
/// <param name="PlayerShots">The shots fired by the player.</param>
/// <param name="EnemyShots">The shots fired by the enemies.</param>
/// <param name="ShouldContinue">Indicates whether the game should continue.</param>
public record FrameSnapshot(
    IBattleStar Player,
    IEnumerable<IBattleStar> Enemies,
    IEnumerable<IShot> PlayerShots,
    IEnumerable<IShot> EnemyShots,
    bool ShouldContinue
);