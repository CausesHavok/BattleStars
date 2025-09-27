
using FluentAssertions;
using Moq;
using BattleStars.Core;
using BattleStars.Logic;
using BattleStars.Shots;

namespace BattleStars.Tests.Logic;

public class GameStateTest
{
    #region Construction

    [Fact]
    public void GivenNullContext_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var player = new Mock<IBattleStar>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(null!, player, playerShots, enemies, enemyShots);
        act.Should().Throw<ArgumentNullException>().WithMessage("*context*");
    }

    [Fact]
    public void GivenNullPlayer_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var context = new Mock<IContext>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, null!, playerShots, enemies, enemyShots);
        act.Should().Throw<ArgumentNullException>().WithMessage("*player*");
    }

    [Fact]
    public void GivenNullPlayerShots_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, player, null!, enemies, enemyShots);
        act.Should().Throw<ArgumentNullException>().WithMessage("*playerShots*");
    }

    [Fact]
    public void GivenNullEnemies_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = new List<IShot>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, player, playerShots, null!, enemyShots);
        act.Should().Throw<ArgumentNullException>().WithMessage("*enemies*");
    }

    [Fact]
    public void GivenNullEnemyShots_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar>();

        Action act = () => new GameState(context, player, playerShots, enemies, null!);
        act.Should().Throw<ArgumentNullException>().WithMessage("*enemyShots*");
    }

    [Fact]
    public void GivenValidInputs_WhenConstructed_ThenPropertiesAreSet()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot>();

        var gameState = new GameState(context, player, playerShots, enemies, enemyShots);

        gameState.Context.Should().Be(context);
        gameState.Player.Should().Be(player);
        gameState.PlayerShots.Should().BeSameAs(playerShots);
        gameState.Enemies.Should().BeSameAs(enemies);
        gameState.EnemyShots.Should().BeSameAs(enemyShots);
    }

    #endregion

    #region CrossValidation

    [Fact]
    public void GivenPlayerIsInEnemies_WhenConstructed_ThenThrowsInvalidOperationException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar> { player };
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player cannot be an enemy*");
    }

    [Fact]
    public void GivenPlayerShotsIntersectEnemyShots_WhenConstructed_ThenThrowsInvalidOperationException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = new Mock<IShot>().Object;
        var playerShots = new List<IShot> { shot };
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot> { shot };

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player shots cannot be enemy shots*");
    }

    [Fact]
    public void GivenEnemiesWithDuplicates_WhenConstructed_ThenThrowsInvalidOperationException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var enemy1 = new Mock<IBattleStar>().Object;
        var enemies = new List<IBattleStar> { enemy1, enemy1 };
        var playerShots = new List<IShot>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().Throw<InvalidOperationException>().WithMessage("*Enemies list contains duplicate entries*");
    }

    [Fact]
    public void GivenPlayerShotsWithDuplicates_WhenConstructed_ThenThrowsInvalidOperationException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = new Mock<IShot>().Object;
        var playerShots = new List<IShot> { shot, shot };
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot>();

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().Throw<InvalidOperationException>().WithMessage("*Player shots list contains duplicate entries*");
    }

    [Fact]
    public void GivenEnemyShotsWithDuplicates_WhenConstructed_ThenThrowsInvalidOperationException()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var shot = new Mock<IShot>().Object;
        var playerShots = new List<IShot>();
        var enemies = new List<IBattleStar>();
        var enemyShots = new List<IShot> { shot, shot };

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().Throw<InvalidOperationException>().WithMessage("*Enemy shots list contains duplicate entries*");
    }

    [Fact]
    public void GivenNoDuplicatesOrConflicts_WhenConstructed_ThenDoesNotThrow()
    {
        var context = new Mock<IContext>().Object;
        var player = new Mock<IBattleStar>().Object;
        var enemy1 = new Mock<IBattleStar>().Object;
        var enemy2 = new Mock<IBattleStar>().Object;
        var enemies = new List<IBattleStar> { enemy1, enemy2 };
        var shot1 = new Mock<IShot>().Object;
        var shot2 = new Mock<IShot>().Object;
        var playerShots = new List<IShot> { shot1 };
        var enemyShots = new List<IShot> { shot2 };

        Action act = () => new GameState(context, player, playerShots, enemies, enemyShots);
        act.Should().NotThrow();
    }

    #endregion
}