namespace BattleStars.Domain.Interfaces;

/// <summary>
/// Controls enemy entities in the game.
/// </summary>
public interface IEnemyController
{
    void UpdateEnemies(IContext context, IGameState gameState);
}