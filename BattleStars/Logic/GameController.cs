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
    private readonly IBattleStar _player;
    private readonly List<IBattleStar> _enemies;
    private List<IShot> _playerShots;
    private List<IShot> _enemyShots;
    private readonly IInputHandler _inputHandler;
    private readonly IBoundaryChecker _boundaryChecker;

    public GameController(IGameState gameState, IInputHandler inputHandler, IBoundaryChecker boundaryChecker)
    {
        ArgumentNullException.ThrowIfNull(gameState, nameof(gameState));
        ArgumentNullException.ThrowIfNull(gameState.Player, nameof(gameState.Player));
        ArgumentNullException.ThrowIfNull(gameState.Enemies, nameof(gameState.Enemies));
        ArgumentNullException.ThrowIfNull(gameState.PlayerShots, nameof(gameState.PlayerShots));
        ArgumentNullException.ThrowIfNull(gameState.EnemyShots, nameof(gameState.EnemyShots));

        _player = gameState.Player;
        _enemies = gameState.Enemies;
        _playerShots = gameState.PlayerShots;
        _enemyShots = gameState.EnemyShots;

        ArgumentNullException.ThrowIfNull(inputHandler, nameof(inputHandler));
        ArgumentNullException.ThrowIfNull(boundaryChecker, nameof(boundaryChecker));
        _inputHandler = inputHandler;
        _boundaryChecker = boundaryChecker;

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
    /// </remarks>
    public bool RunFrame(IContext context)
    {
        if (_inputHandler.ShouldExit())
        {
            return false;
        }

        UpdatePlayer(context);
        UpdateEnemies(context);
        UpdateShots();
        CollisionHandling();
        BoundaryHandling();

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
        _player.Move(context);

        // Handle shooting
        if (_inputHandler.ShouldShoot())
        {
            var shot = _player.Shoot(context);
            if (shot != null)
            {
                _playerShots.AddRange(shot);
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
        foreach (var enemy in _enemies.ToList())
        {
            if (enemy.IsDestroyed)
            {
                _enemies.Remove(enemy);
                continue;
            }

            enemy.Move(context);
            var shot = enemy.Shoot(context);
            if (shot != null)
            {
                _enemyShots.AddRange(shot);
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
        foreach (var shot in _playerShots)
        {
            shot.Update();
        }

        foreach (var shot in _enemyShots)
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
        foreach (var shot in _playerShots.ToList())
        {
            foreach (var enemy in _enemies)
            {
                if (CollisionChecker.CheckBattleStarShotCollision(enemy, shot))
                {
                    enemy.TakeDamage(shot.Damage);
                    _playerShots.Remove(shot);
                    break;
                }
            }
        }

        foreach (var shot in _enemyShots.ToList())
        {
            if (CollisionChecker.CheckBattleStarShotCollision(_player, shot))
            {
                _player.TakeDamage(shot.Damage);
                _enemyShots.Remove(shot);
                if (_player.IsDestroyed) break;
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
        foreach (var shot in _playerShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                _playerShots.Remove(shot);
            }
        }

        foreach (var shot in _enemyShots.ToList())
        {
            if (_boundaryChecker.IsOutsideXBounds(shot.Position.X) || _boundaryChecker.IsOutsideYBounds(shot.Position.Y))
            {
                _enemyShots.Remove(shot);
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
        return !_player.IsDestroyed;
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
            Player: _player,
            Enemies: _enemies,
            PlayerShots: _playerShots,
            EnemyShots: _enemyShots,
            ShouldContinue: !_player.IsDestroyed
        );
    }
}