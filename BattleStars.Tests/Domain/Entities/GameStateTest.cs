using FluentAssertions;
using Moq;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.Entities;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Domain.Entities;

public class GameStateTest
{
    #region Construction

    [Fact]
    public void GivenNullContext_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Given
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();

        // When
        Action act = () => new GameState(null!, player, playerShots, enemies, enemyShots);

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*context*");
    }

    [Fact]
    public void GivenNullPlayer_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();

        // When
        Action act = () => new GameState(context, null!, playerShots, enemies, enemyShots);

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*player*");
    }

    [Fact]
    public void GivenNullEnemies_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemyShots = ShotFactory.CreateEmptyShotList();

        // When
        Action act = () => new GameState(context, player, playerShots, null!, enemyShots);

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*enemies*");
    }

    [Fact]
    public void GivenValidInputs_WhenConstructed_ThenPropertiesAreSet()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();

        // When
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // Then
        gameState.Context.Should().Be(context);
        gameState.Player.Should().Be(player);
        gameState.PlayerShots.Should().BeSameAs(playerShots);
        gameState.Enemies.Should().BeSameAs(enemies);
        gameState.EnemyShots.Should().BeSameAs(enemyShots);
    }

    #endregion

    #region PropertySetters

    [Fact]
    public void GivenNullContext_WhenSet_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Context = null!;

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*Value*");
    }

    [Fact]
    public void GivenNullPlayer_WhenSet_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Player = null!;

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*Value*");
    }

    [Fact]
    public void GivenNullPlayerShots_WhenSet_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.PlayerShots = null!;

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*Value*");
    }

    [Fact]
    public void GivenNullEnemies_WhenSet_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Enemies = null!;

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*Value*");
    }

    [Fact]
    public void GivenNullEnemyShots_WhenSet_ThenThrowsArgumentNullException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.EnemyShots = null!;

        // Then
        act.Should().Throw<ArgumentNullException>().WithMessage("*Value*");
    }

    [Fact]
    public void GivenValidValues_WhenSet_ThenPropertiesAreUpdated()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        var newContext = new Mock<IContext>().Object;
        var newPlayer = new Mock<IBattleStar>().Object;
        var newPlayerShots = new List<IShot> { ShotFactory.CreateNoOpShot() };
        var newEnemies = new List<IBattleStar> { new Mock<IBattleStar>().Object };
        var newEnemyShots = new List<IShot> { ShotFactory.CreateNoOpShot() };

        // When
        gameState.Context = newContext;
        gameState.Player = newPlayer;
        gameState.PlayerShots = newPlayerShots;
        gameState.Enemies = newEnemies;
        gameState.EnemyShots = newEnemyShots;

        // Then
        gameState.Context.Should().Be(newContext);
        gameState.Player.Should().Be(newPlayer);
        gameState.PlayerShots.Should().BeSameAs(newPlayerShots);
        gameState.Enemies.Should().BeSameAs(newEnemies);
        gameState.EnemyShots.Should().BeSameAs(newEnemyShots);
    }

    #endregion

    #region Validation

    [Fact]
    public void GivenPlayerIsInEnemies_WhenValidate_ThenThrowsInvalidOperationException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar> { player };
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player cannot be an enemy*");
    }

    [Fact]
    public void GivenPlayerShotsIntersectEnemyShots_WhenValidate_ThenThrowsInvalidOperationException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = ShotFactory.CreateNoOpShot();
        var playerShots = new List<IShot> { shot };
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot> { shot };
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player shots cannot be enemy shots*");
    }

    [Fact]
    public void GivenEnemiesWithDuplicates_WhenValidate_ThenThrowsInvalidOperationException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var enemy1 = new Mock<IBattleStar>().Object;
        var enemies = new List<IBattleStar> { enemy1, enemy1 };
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().Throw<InvalidOperationException>().WithMessage("*Enemies list contains duplicate entries*");
    }

    [Fact]
    public void GivenPlayerShotsWithDuplicates_WhenValidate_ThenThrowsInvalidOperationException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = ShotFactory.CreateNoOpShot();
        var playerShots = new List<IShot> { shot, shot };
        var enemies = new List<IBattleStar>();
        var enemyShots = ShotFactory.CreateEmptyShotList();
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player shots list contains duplicate entries*");
    }

    [Fact]
    public void GivenEnemyShotsWithDuplicates_WhenValidate_ThenThrowsInvalidOperationException()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = ShotFactory.CreateNoOpShot();
        var playerShots = ShotFactory.CreateEmptyShotList();
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot> { shot, shot };
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().Throw<InvalidOperationException>().WithMessage("*Enemy shots list contains duplicate entries*");
    }

    [Fact]
    public void GivenNoDuplicatesOrConflicts_WhenValidate_ThenDoesNotThrow()
    {
        // Given
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var enemy1 = new Mock<IBattleStar>().Object;
        var enemy2 = new Mock<IBattleStar>().Object;
        var enemies = new List<IBattleStar> { enemy1, enemy2 };
        var shot1 = ShotFactory.CreateNoOpShot();
        var shot2 = ShotFactory.CreateNoOpShot();
        var playerShots = new List<IShot> { shot1 };
        var enemyShots = new List<IShot> { shot2 };
        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        // When
        Action act = () => gameState.Validate();

        // Then
        act.Should().NotThrow();
    }

    #endregion
}