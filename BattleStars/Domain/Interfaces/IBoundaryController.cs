namespace BattleStars.Domain.Interfaces;
/// <summary>
/// Controls boundary enforcement in the game.
/// </summary>
internal interface IBoundaryController
{
    void EnforceBoundaries(IGameState gameState);
}