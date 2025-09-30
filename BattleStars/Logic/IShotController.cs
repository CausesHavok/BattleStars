namespace BattleStars.Logic;

/// <summary>
/// Controls the behavior of shots in the game.
/// </summary>
public interface IShotController
{
    void UpdateShots(IGameState gameState);
}
