using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Utilities;
namespace BattleStars.Application.Services;

/// <summary>
/// Controls collision detection and response in the game.
/// </summary>
public class CollisionController : ICollisionController
{

    public CollisionController(ICollisionChecker collisionChecker)
    {
        Guard.NotNull(collisionChecker, nameof(collisionChecker));
        _collisionChecker = collisionChecker;
    }

    private ICollisionChecker _collisionChecker;
    /// <summary>
    /// Handles collisions between shots and battle stars.
    /// </summary>
    /// <remarks>
    /// This method checks for collisions between player shots and enemies, as well as enemy shots and the player.
    /// </remarks>
    public void HandleCollisions(IGameState gameState)
    {
        Guard.NotNull(gameState, nameof(gameState));
        Guard.NotNull(gameState.Player, nameof(gameState.Player));
        Guard.NotNull(gameState.Enemies, nameof(gameState.Enemies));
        Guard.NotNull(gameState.PlayerShots, nameof(gameState.PlayerShots));
        Guard.NotNull(gameState.EnemyShots, nameof(gameState.EnemyShots));

        HandleEnemyCollisions(gameState);
        HandlePlayerCollision(gameState);
    }

    /// <summary>
    /// Handles collisions between player shots and enemies.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all player shots and checks for collisions with each enemy.
    /// If a collision is detected, the enemy takes damage and the shot is removed.
    /// If the enemy is destroyed, it is also removed from the game state.
    /// </remarks>
    private void HandlePlayerCollision(IGameState gameState)
    {
        foreach (var shot in gameState.PlayerShots.ToList())
        {
            foreach (var enemy in gameState.Enemies.ToList())
            {
                if (_collisionChecker.CheckBattleStarShotCollision(enemy, shot))
                {
                    enemy.TakeDamage(shot.Damage);
                    gameState.PlayerShots.Remove(shot);
                    if (enemy.IsDestroyed) gameState.Enemies.Remove(enemy);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Handles collisions between enemy shots and the player.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all enemy shots and checks for collisions with the player.
    /// If a collision is detected, the player takes damage and the shot is removed.
    /// If the player is destroyed, the process stops.
    /// </remarks>
    private void HandleEnemyCollisions(IGameState gameState)
    {
        foreach (var shot in gameState.EnemyShots.ToList())
        {
            if (_collisionChecker.CheckBattleStarShotCollision(gameState.Player, shot))
            {
                gameState.Player.TakeDamage(shot.Damage);
                gameState.EnemyShots.Remove(shot);
                if (gameState.Player.IsDestroyed) break;
            }
        }
    }
}
