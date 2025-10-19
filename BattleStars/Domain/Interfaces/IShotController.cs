namespace BattleStars.Domain.Interfaces;

/// <summary>
/// Controls the behavior of shots in the game.
/// </summary>
internal interface IShotController
{
    void UpdateShots(IGameState gameState);
}
