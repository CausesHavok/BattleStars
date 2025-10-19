namespace BattleStars.Domain.Interfaces;
/// <summary>
/// Controls collision detection and response in the game.
/// </summary>
internal interface ICollisionController
{
    void HandleCollisions(IGameState gameState);
}