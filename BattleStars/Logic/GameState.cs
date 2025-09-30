using BattleStars.Core;
using BattleStars.Shots;
namespace BattleStars.Logic;

/// <summary>
/// Represents the current state of the game, including context, player, enemies, and shots.
/// </summary>
/// <remarks>
/// This class encapsulates all the necessary components to represent the game's state at any given moment.
/// </remarks>
public class GameState(
    IContext context,
    IBattleStar player,
    List<IShot> playerShots,
    List<IBattleStar> enemies,
    List<IShot> enemyShots) : IGameState
{
    private IContext _context = GuardNotNull(context, nameof(context));
    private IBattleStar _player = GuardNotNull(player, nameof(player));
    private List<IBattleStar> _enemies = GuardNotNull(enemies, nameof(enemies));
    private List<IShot> _playerShots = GuardNotNull(playerShots, nameof(playerShots));
    private List<IShot> _enemyShots = GuardNotNull(enemyShots, nameof(enemyShots));
    public IContext Context
    {
        get => _context;
        set => _context = GuardNotNull(value, nameof(Context));
    }
    public IBattleStar Player
    {
        get => _player;
        set => _player = GuardNotNull(value, nameof(Player));
    }
    public List<IBattleStar> Enemies
    {
        get => _enemies;
        set => _enemies = GuardNotNull(value, nameof(Enemies));
    }
    public List<IShot> PlayerShots
    {
        get => _playerShots;
        set => _playerShots = GuardNotNull(value, nameof(PlayerShots));
    }
    public List<IShot> EnemyShots
    {
        get => _enemyShots;
        set => _enemyShots = GuardNotNull(value, nameof(EnemyShots));
    }


    /// <summary>
    /// Validates the internal consistency of the game state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if any validation checks fail.</exception>
    /// <remarks>
    /// This method performs cross-validation checks to ensure that the game state is logically consistent.
    /// It checks for conditions such as the player not being listed as an enemy,
    /// and that there are no duplicate entries in the enemies or shots lists.
    /// </remarks>
    public void Validate()
    {
        if (Enemies.Contains(Player))
        {
            throw new InvalidOperationException("Player cannot be an enemy.");
        }

        if (PlayerShots.Intersect(EnemyShots).Any())
        {
            throw new InvalidOperationException("Player shots cannot be enemy shots.");
        }

        if (Enemies.Distinct().Count() != Enemies.Count)
        {
            throw new InvalidOperationException("Enemies list contains duplicate entries.");
        }

        if (PlayerShots.Distinct().Count() != PlayerShots.Count)
        {
            throw new InvalidOperationException("Player shots list contains duplicate entries.");
        }

        if (EnemyShots.Distinct().Count() != EnemyShots.Count)
        {
            throw new InvalidOperationException("Enemy shots list contains duplicate entries.");
        }
    }

    /// <summary>
    /// Guards against null values, throwing an ArgumentNullException if the value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="name">The name of the parameter being checked.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
    /// <remarks>
    /// This method is a utility to ensure that required parameters are not null, improving code safety
    /// and reducing boilerplate null-checking code throughout the class.
    /// </remarks>
    private static T GuardNotNull<T>(T value, string name)
    where T : class
    {
        return value ?? throw new ArgumentNullException(name);
    }
}