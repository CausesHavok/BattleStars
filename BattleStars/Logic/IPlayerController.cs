using BattleStars.Core;
namespace BattleStars.Logic;

/// <summary>
/// Controls the player character (BattleStar) based on input and game state.
/// </summary>
public interface IPlayerController
{
    void UpdatePlayer(IContext context, IInputHandler inputHandler, IGameState gameState);
}