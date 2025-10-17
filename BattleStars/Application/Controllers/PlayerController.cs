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
    /// <param name="context">The current game context, including player direction.</param>
    /// <param name="inputHandler">The input handler to retrieve player commands.</param>
    /// <param name="gameState">The current game state, including the player and active
    /// shots.</param>
    /// <remarks>
    /// This method first validates the input parameters to ensure they are not null.
    /// It then updates the player's movement based on the input handler's movement direction.
    /// If the input handler indicates that the player should shoot, it retrieves the shots
    /// from the player and adds them to the game state's active shots.
    /// </remarks>
    public void UpdatePlayer(IContext context, IInputHandler inputHandler, IGameState gameState)
    {
        MovePlayer(context, inputHandler, gameState.Player);
        HandleShooting(context, inputHandler, gameState);
    }

    /// <summary>
    /// Moves the player based on input direction.
    /// </summary>
    /// <param name="context">The current game context.</param>
    /// <param name="inputHandler">The input handler to retrieve movement direction.</param>
    /// <param name="player">The player character to move.</param>
    /// <remarks>
    /// This method retrieves the movement direction from the input handler,
    /// updates the context with this direction, and then calls the player's Move method
    /// to update its position.
    /// </remarks>
    private void MovePlayer(IContext context, IInputHandler inputHandler, IBattleStar player)
    {
        context.PlayerDirection = inputHandler.GetMovement();
        player.Move(context);
    }

    /// <summary>
    /// Handles the shooting action of the player.
    /// </summary>
    /// <param name="context">The current game context.</param>
    /// <param name="inputHandler">The input handler to check for shooting command.</param>
    /// <param name="gameState">The current game state to add new shots.</param>
    /// <remarks>
    /// This method checks if the input handler indicates that the player should shoot.
    /// If so, it retrieves the shots from the player and adds them to the game state's active shots.
    /// </remarks>
    private void HandleShooting(IContext context, IInputHandler inputHandler, IGameState gameState)
    {
        if (inputHandler.ShouldShoot())
        {
            var shots = gameState.Player.Shoot(context);
            if (shots != null)
            {
                gameState.PlayerShots.AddRange(shots);
            }
        }
    }
}