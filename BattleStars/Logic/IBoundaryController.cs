namespace BattleStars.Logic;
/// <summary>
/// Controls boundary enforcement in the game.
/// </summary>
public interface IBoundaryController
{
    void EnforceBoundaries(IGameState gameState);
}