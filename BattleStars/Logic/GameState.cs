using BattleStars.Core;
using BattleStars.Shots;
namespace BattleStars.Logic;

/// <summary>
/// Represents the current state of the game, including context, player, enemies, and shots.
/// </summary>
/// <remarks>
/// This class encapsulates all the necessary components to represent the game's state at any given moment.
/// </remarks>
public class GameState : IGameState
{
    public IContext Context { get; }
    public IBattleStar Player { get; }
    public List<IBattleStar> Enemies { get; } = [];
    public List<IShot> PlayerShots { get; } = [];
    public List<IShot> EnemyShots { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="IImmutableGameState"/> class.
    /// </summary>
    /// <param name="context">The game context.</param>
    /// <param name="inputHandler">The input handler.</param>
    /// <param name="player">The player character.</param>
    /// <param name="playerShots">The shots fired by the player.</param>
    /// <param name="enemies">The enemy characters.</param>
    /// <param name="enemyShots">The shots fired by the enemies.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    /// <remarks>
    /// This constructor initializes the game state with the provided context, input handler, player, enemies, and shots.
    /// It ensures that all components are properly set up for managing the game's state.
    /// </remarks>
    public GameState(
        IContext context,
        IBattleStar player,
        List<IShot> playerShots,
        List<IBattleStar> enemies,
        List<IShot> enemyShots)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(enemies, nameof(enemies));
        ArgumentNullException.ThrowIfNull(enemyShots, nameof(enemyShots));
        ArgumentNullException.ThrowIfNull(player, nameof(player));
        ArgumentNullException.ThrowIfNull(playerShots, nameof(playerShots));

        Context = context;
        Enemies = enemies;
        EnemyShots = enemyShots;
        Player = player;
        PlayerShots = playerShots;

        CrossValidate();
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
    private void CrossValidate()
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