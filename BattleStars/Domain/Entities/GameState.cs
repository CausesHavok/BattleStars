using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
namespace BattleStars.Domain.Entities;

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
    private IContext _context = Guard.NotNull(context, nameof(context));
    private IBattleStar _player = Guard.NotNull(player, nameof(player));
    private List<IBattleStar> _enemies = Guard.NotNull(enemies, nameof(enemies));
    private List<IShot> _playerShots = playerShots;
    private List<IShot> _enemyShots = enemyShots;
    public IContext Context
    {
        get => _context;
        set => _context = Guard.NotNull(value, nameof(Context));
    }
    public IBattleStar Player
    {
        get => _player;
        set => _player = Guard.NotNull(value, nameof(Player));
    }
    public List<IBattleStar> Enemies
    {
        get => _enemies;
        set => _enemies = Guard.NotNull(value, nameof(Enemies));
    }
    public List<IShot> PlayerShots
    {
        get => _playerShots;
        set => _playerShots = Guard.NotNull(value, nameof(PlayerShots));
    }
    public List<IShot> EnemyShots
    {
        get => _enemyShots;
        set => _enemyShots = Guard.NotNull(value, nameof(EnemyShots));
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
}