using BattleStars.Domain.Interfaces;
namespace BattleStars.Application.Controllers;

/// <summary>
/// Controls the behavior of shots in the game.
/// </summary>
/// <remarks>
/// This class is responsible for updating the positions of all active shots in the game state.
/// It iterates through both player and enemy shots, calling their update methods to reflect movement.
/// </remarks>
public class ShotController : IShotController
{
    /// <summary>
    /// Updates the positions of all active shots.
    /// </summary>
    /// <remarks>
    /// This method iterates through all player and enemy shots, updating their positions.
    /// </remarks>
    public void UpdateShots(IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState, nameof(gameState));
        ArgumentNullException.ThrowIfNull(gameState.PlayerShots, nameof(gameState.PlayerShots));
        ArgumentNullException.ThrowIfNull(gameState.EnemyShots, nameof(gameState.EnemyShots));

        HandleShots(gameState.PlayerShots);
        HandleShots(gameState.EnemyShots);
    }

    /// <summary>
    /// Updates the positions of the given shots.
    /// </summary>
    /// <param name="shots">The shots to update.</param>
    /// <remarks>
    /// This method iterates through the provided shots and calls their Update method to
    /// update their positions.
    /// </remarks>
    private void HandleShots(IEnumerable<IShot> shots)
    {
        foreach (var shot in shots)
        {
            shot.Update();
        }
    }
}

