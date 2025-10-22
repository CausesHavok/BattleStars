using BattleStars.Domain.Interfaces;
namespace BattleStars.Application.Controllers;

/// <summary>
/// Controls the player character (BattleStar) based on input and game state.
/// </summary>
/// <remarks>
/// This class is responsible for updating the player's position and handling shooting actions.
/// It interacts with the input handler to get movement directions and shooting commands,
/// and updates the game state accordingly.
/// </remarks>
internal class PlayerController : IPlayerController
{
    /// <summary>
    /// Updates the player character based on input and game state.
    /// </summary>
    /// <param name="inputHandler">The input handler to retrieve player commands.</param>
    /// <param name="gameState">The current game state, including the player and active
    /// shots.</param>
    /// <remarks>
    /// This method moves the player based on input direction and handles shooting actions.
    /// It updates the game state with any new shots fired by the player.
    /// </remarks>
    public void UpdatePlayer(IInputHandler inputHandler, IGameState gameState)
    {
        MovePlayer(inputHandler, gameState);
        HandleShooting(inputHandler, gameState);
    }

    /// <summary>
    /// Moves the player based on input direction.
    /// </summary>
    /// <param name="inputHandler">The input handler to retrieve movement direction.</param>
    /// <param name="gameState">The current game state, including the player character.</param>
    /// <remarks>
    /// This method retrieves the movement direction from the input handler,
    /// updates the game state with this direction, and then calls the player's Move method
    /// to update its position.
    /// </remarks>
    private void MovePlayer(IInputHandler inputHandler, IGameState gameState)
    {
        gameState.Context.PlayerDirection = inputHandler.GetMovement();
        gameState.Player.Move(gameState.Context);
    }

    /// <summary>
    /// Handles the shooting action of the player.
    /// </summary>
    /// <param name="inputHandler">The input handler to check for shooting command.</param>
    /// <param name="gameState">The current game state to add new shots.</param>
    /// <remarks>
    /// This method checks if the input handler indicates that the player should shoot.
    /// If so, it retrieves the shots from the player and adds them to the game state's active shots.
    /// </remarks>
    private void HandleShooting(IInputHandler inputHandler, IGameState gameState)
    {
        if (inputHandler.ShouldShoot())
        {
            var shots = gameState.Player.Shoot(gameState.Context);
            if (shots != null)
            {
                gameState.PlayerShots.AddRange(shots);
            }
        }
    }
}