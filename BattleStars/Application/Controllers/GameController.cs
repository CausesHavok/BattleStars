using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
namespace BattleStars.Application.Controllers;

/// <summary>
/// Main game controller responsible for managing game state and logic.
/// </summary>
/// <remarks>
/// This class handles player and enemy updates, shot management, collision detection,
/// and boundary checks. It maintains the current state of the game and provides
/// snapshots of the game state for rendering or other purposes.
/// </remarks>
public class GameController : IGameController
{
    private readonly IGameState _gameState;
    private readonly IInputHandler _inputHandler;
    private readonly IPlayerController _playerController;
    private readonly IEnemyController _enemyController;
    private readonly IShotController _shotController;
    private readonly IBoundaryController _boundaryController;
    private readonly ICollisionController _collisionController;
    private readonly IContext _context;

    internal GameController(
        IGameState gameState,
        IPlayerController playerController,
        IEnemyController enemyController,
        IShotController shotController,
        IBoundaryController boundaryController,
        ICollisionController collisionController,
        IInputHandler inputHandler,
        IContext context)
    {
        _gameState = gameState;
        _playerController = playerController;
        _enemyController = enemyController;
        _shotController = shotController;
        _boundaryController = boundaryController;
        _collisionController = collisionController;
        _inputHandler = inputHandler;
        _context = context;
    }

    /// <summary>
    /// Runs a single frame of the game, updating all entities and handling game logic.
    /// </summary>
    /// <param name="context">The game context containing frame-specific information.</param>
    /// <returns>True if the game should continue, false if it should end.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the context is null.</exception>
    /// <remarks>
    /// This method processes player input, updates the positions of all entities,
    /// checks for collisions, and removes any entities that are out of bounds or destroyed.
    /// It returns a boolean indicating whether the game should continue running.
    /// 
    /// The guiding principles for the simulation are:
    /// Move before you shoot.
    /// Shoot before you check for collisions.
    /// Check boundaries before collisions. - boundary checks are cheaper than collision checks.
    /// Short-circuit collisions - if an entity is destroyed, don't check it against other entities.
    /// </remarks>
    public bool RunFrame()
    {
        if (_inputHandler.ShouldExit()) return false;

        _shotController.UpdateShots(_gameState);
        _playerController.UpdatePlayer(_context, _inputHandler, _gameState);
        _enemyController.UpdateEnemies(_context, _gameState);
        _boundaryController.EnforceBoundaries(_gameState);
        _collisionController.HandleCollisions(_gameState);

        _gameState.Validate();
        return ShouldContinue();
    }

    /// <summary>
    /// Determines whether the game should continue.
    /// </summary>
    /// <remarks>
    /// This method checks if the player is still alive.
    /// </remarks>
    private bool ShouldContinue() => !_gameState.Player.IsDestroyed;

    /// <summary>
    /// Gets a snapshot of the current game state.
    /// </summary>
    /// <remarks>
    /// This method captures the current state of the player, enemies, and shots.
    /// </remarks>
    public FrameSnapshot GetFrameSnapshot()
    {
        return new FrameSnapshot(
            Player: _gameState.Player,
            Enemies: _gameState.Enemies,
            PlayerShots: _gameState.PlayerShots,
            EnemyShots: _gameState.EnemyShots,
            ShouldContinue: ShouldContinue()
        );
    }
}