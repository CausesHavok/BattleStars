namespace BattleStars.Domain.Interfaces;

/// <summary>
/// Controls enemy entities in the game.
/// </summary>
internal interface IEnemyController
{
    void UpdateEnemies(IGameState gameState);
}