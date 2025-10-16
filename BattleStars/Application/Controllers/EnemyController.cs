using BattleStars.Domain.Interfaces;
namespace BattleStars.Application.Controllers;

/// <summary>
/// Controls enemy entities in the game.
/// </summary>
/// <remarks>
/// This class is responsible for updating the position of each enemy and handling their shooting.
/// </remarks>
internal class EnemyController : IEnemyController
{

    /// <summary>
    /// Updates all enemies in the game.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <remarks>
    /// This method updates the position of each enemy and handles their shooting.
    /// </remarks>
    public void UpdateEnemies(IContext context, IGameState gameState)
    {
        MoveEnemies(context, gameState);
        HandleShooting(context, gameState);
    }

    /// <summary>
    /// Handles the shooting action of all enemies.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all enemies, calling their Shoot method.
    /// If an enemy shoots, the resulting shots are added to the game state's enemy shots.
    /// </remarks>
    private void HandleShooting(IContext context, IGameState gameState)
    {
        foreach (var enemy in gameState.Enemies)
        {
            var shot = enemy.Shoot(context);
            if (shot != null) gameState.EnemyShots.AddRange(shot);
        }
    }

    /// <summary>
    /// Moves all enemies based on the game context.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <param name="gameState">The current game state.</param>
    /// <remarks>
    /// This method iterates through all enemies and calls their Move method to update their positions.
    /// </remarks>
    private void MoveEnemies(IContext context, IGameState gameState)
    {
        foreach (var enemy in gameState.Enemies)
        {
            enemy.Move(context);
        }
    }
}