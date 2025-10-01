namespace BattleStars.Domain.Interfaces;
/// <summary>
/// Controls collision detection and response in the game.
/// </summary>
public interface ICollisionController
{
    void HandleCollisions(IGameState gameState);
}