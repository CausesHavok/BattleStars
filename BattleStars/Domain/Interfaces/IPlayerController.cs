namespace BattleStars.Domain.Interfaces;

/// <summary>
/// Controls the player character (BattleStar) based on input and game state.
/// </summary>
internal interface IPlayerController
{
    void UpdatePlayer(IInputHandler inputHandler, IGameState gameState);
}