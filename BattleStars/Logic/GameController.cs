using BattleStars.Core;
using BattleStars.Shots;

namespace BattleStars.Logic;

/// <summary>
/// Main game controller responsible for managing game state and logic.
/// </summary>
/// <remarks>
/// This class handles player and enemy updates, shot management, collision detection,
/// and boundary checks. It maintains the current state of the game and provides
/// snapshots of the game state for rendering or other purposes.
/// </remarks>
public class GameController
{
    private readonly IGameState _gameState;
    private readonly IInputHandler _inputHandler;
    private readonly IBoundaryChecker _boundaryChecker;

    public GameController(IGameState mutableGameState, IInputHandler inputHandler, IBoundaryChecker boundaryChecker)
    {
        ArgumentNullException.ThrowIfNull(mutableGameState, nameof(mutableGameState));
        mutableGameState.Validate();
        ArgumentNullException.ThrowIfNull(inputHandler, nameof(inputHandler));
        ArgumentNullException.ThrowIfNull(boundaryChecker, nameof(boundaryChecker));
        _inputHandler = inputHandler;
        _boundaryChecker = boundaryChecker;
        _gameState = mutableGameState;
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
    public bool RunFrame(IContext context)
    {
        if (_inputHandler.ShouldExit())
        {
            return false;
        }

        UpdateShots();
        UpdatePlayer(context);
        UpdateEnemies(context);
        BoundaryHandling();
        CollisionHandling();
        
        return ShouldContinue();
    }

    /// <summary>
    /// Updates the player based on input and game context.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <remarks>
    /// This method updates the player's position based on input and handles shooting.
    /// If the player shoots, the resulting shots are added to the player's shot list.
    /// </remarks>
    private void UpdatePlayer(IContext context)
    {
        // Update player direction based on input
        context.PlayerDirection = _inputHandler.GetMovement();
        _gameState.Player.Move(context);

        // Handle shooting
        if (_inputHandler.ShouldShoot())
        {
            var shot = _gameState.Player.Shoot(context);
            if (shot != null)
            {
                _gameState.PlayerShots.AddRange(shot);
            }
        }
    }

    /// <summary>
    /// Updates all enemies in the game.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <remarks>
    /// This method updates the position of each enemy and handles their shooting.
    /// </remarks>
    private void UpdateEnemies(IContext context)
    {
        foreach (var enemy in _gameState.Enemies.ToList())
        {
            enemy.Move(context);
            var shot = enemy.Shoot(context);
            if (shot != null)
            {
                _gameState.EnemyShots.AddRange(shot);
            }
        }
        ;
    }

    /// <summary>
    /// Updates the positions of all active shots.
    /// </summary>
    /// <remarks>
    /// This method iterates through all player and enemy shots, updating their positions.
    /// </remarks>
    private void UpdateShots()
    {
        foreach (var shot in _gameState.PlayerShots)
        {
            shot.Update();
        }

        foreach (var shot in _gameState.EnemyShots)
        {
            shot.Update();
        }
    }

    /// <summary>
    /// Handles collisions between shots and battle stars.
    /// </summary>
    /// <remarks>
    /// This method checks for collisions between player shots and enemies, as well as enemy shots and the player.
    /// </remarks>
    private void CollisionHandling()
    {
        foreach (var shot in _gameState.PlayerShots.ToList())
        {
            foreach (var enemy in _gameState.Enemies.ToList())
            {
                if (CollisionChecker.CheckBattleStarShotCollision(enemy, shot))
                {
                    enemy.TakeDamage(shot.Damage);
                    _gameState.PlayerShots.Remove(shot);
                    if (enemy.IsDestroyed) _gameState.Enemies.Remove(enemy);
                    break;
                }
            }
        }

        foreach (var shot in _gameState.EnemyShots.ToList())
        {
            if (CollisionChecker.CheckBattleStarShotCollision(_gameState.Player, shot))
            {
                _gameState.Player.TakeDamage(shot.Damage);
                _gameState.EnemyShots.Remove(shot);
                if (_gameState.Player.IsDestroyed) break;
            }
        }
    }

    /// <summary>
    /// Removes shots that are outside the game boundaries.
    /// </summary>
    /// <remarks>
    /// This method checks each shot's position and removes it if it is outside the defined boundaries.
    /// </remarks>
    private void BoundaryHandling()
    {
        foreach (var shot in _gameState.PlayerShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                _gameState.PlayerShots.Remove(shot);
            }
        }

        foreach (var shot in _gameState.EnemyShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                _gameState.EnemyShots.Remove(shot);
            }
        }
    }

    /// <summary>
    /// Determines whether the game should continue.
    /// </summary>
    /// <remarks>
    /// This method checks if the player is still alive.
    /// </remarks>
    private bool ShouldContinue()
    {
        return !_gameState.Player.IsDestroyed;
    }

    /// <summary>
    /// Gets a snapshot of the current game state.
    /// </summary>
    /// <remarks>
    /// This method captures the current state of the player, enemies, and shots.
    /// </remarks>
    public FrameSnapshot GetFrameSnapshot()
    {
        return new FrameSnapshot(
            Player:         _gameState.Player,
            Enemies:        _gameState.Enemies,
            PlayerShots:    _gameState.PlayerShots,
            EnemyShots:     _gameState.EnemyShots,
            ShouldContinue: ShouldContinue()
        );
    }
}