using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Utilities;
namespace BattleStars.Application.Services;

/// <summary>
/// Implements boundary enforcement in the game.
/// </summary>
public class BoundaryController : IBoundaryController
{

    public BoundaryController(IBoundaryChecker boundaryChecker)
    {
        Guard.NotNull(boundaryChecker, nameof(boundaryChecker));
        _boundaryChecker = boundaryChecker;
    }

    private readonly IBoundaryChecker _boundaryChecker;

    /// <summary>
    /// Ensures all game entities remain within the defined game boundaries.
    /// </summary>
    /// <remarks>
    /// This method checks the positions of players, enemies, and shots, adjusting them if they exceed the game area limits.
    /// </remarks>
    public void EnforceBoundaries(IGameState gameState)
    {
        Guard.NotNull(gameState, nameof(gameState));
        Guard.NotNull(gameState.Player, nameof(gameState.Player));
        Guard.NotNull(gameState.Enemies, nameof(gameState.Enemies));
        HandlePlayerShots(gameState);
        HandleEnemyShots(gameState);
    }

    /// <summary>
    /// Handles boundary enforcement for player shots.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all player shots and removes any that are outside the game boundaries
    /// defined by the boundary checker.
    /// </remarks>
    private void HandlePlayerShots(IGameState gameState)
    {
        foreach (var shot in gameState.PlayerShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                gameState.PlayerShots.Remove(shot);
            }
        }
    }

    /// <summary>
    /// Handles boundary enforcement for enemy shots.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all enemy shots and removes any that are outside the game boundaries
    /// defined by the boundary checker.
    /// </remarks>
    private void HandleEnemyShots(IGameState gameState)
    {
        foreach (var shot in gameState.EnemyShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                gameState.EnemyShots.Remove(shot);
            }
        }
    }
}