using FluentAssertions;
using Moq;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Application.Controllers;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Controllers;

public class CollisionControllerTest
{
    #region Null Checks
    [Fact]
    public void GivenNullGameState_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(null!);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState");
    }

    [Fact]
    public void GivenNullPlayer_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns((IBattleStar)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.Player");
    }

    [Fact]
    public void GivenNullEnemies_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns((List<IBattleStar>)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.Enemies");
    }

    [Fact]
    public void GivenNullPlayerShots_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns([]);
        gameStateMock.Setup(g => g.PlayerShots).Returns((List<IShot>)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.PlayerShots");
    }

    [Fact]
    public void GivenNullEnemyShots_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns([]);
        gameStateMock.Setup(g => g.PlayerShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(g => g.EnemyShots).Returns((List<IShot>)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.EnemyShots");
    }

    [Fact]
    public void GivenNullCollisionChecker_WhenCreatingController_ThenThrowsArgumentNullException()
    {
        // When
        var act = () => new CollisionController(null!);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("collisionChecker");
    }

    #endregion

    #region Player Shot vs Enemy Collisions
    [Fact]
    public void GivenPlayerShotCollidesWithEnemy_WhenHandleCollisions_ThenEnemyTakesDamageAndShotRemoved()
    {
        // Given
        var damage = 10;
        var customShot = ShotFactory.CustomShot(
            new PositionalVector2(5, 5),
            new DirectionalVector2(1, 0),
            5,
            damage
        );

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();
        enemyMock.Setup(e => e.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns([enemyMock.Object]);
        gameStateMock.Setup(g => g.PlayerShots).Returns([customShot]);
        gameStateMock.Setup(g => g.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, customShot)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(damage), Times.Once);
        gameStateMock.Object.PlayerShots.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShotDestroysEnemy_WhenHandleCollisions_ThenEnemyAndShotRemoved()
    {
        // Given
        var damage = 10;
        var customShot = ShotFactory.CustomShot(
            new PositionalVector2(5, 5),
            new DirectionalVector2(1, 0),
            5,
            damage
        );

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();
        enemyMock.Setup(e => e.IsDestroyed).Returns(true);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns([enemyMock.Object]);
        gameStateMock.Setup(g => g.PlayerShots).Returns([customShot]);
        gameStateMock.Setup(g => g.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, customShot)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(damage), Times.Once);
        gameStateMock.Object.PlayerShots.Should().BeEmpty();
        gameStateMock.Object.Enemies.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShotDoesNotCollideWithEnemy_WhenHandleCollisions_ThenNoDamageAndShotNotRemoved()
    {
        // Given
        var noOpShot = ShotFactory.CreateNoOpShot(new PositionalVector2(5, 5));

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(false);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns([enemyMock.Object]);
        gameStateMock.Setup(g => g.PlayerShots).Returns([noOpShot]);
        gameStateMock.Setup(g => g.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, noOpShot)).Returns(false);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(It.IsAny<int>()), Times.Never);
        gameStateMock.Object.PlayerShots.Should().Contain(noOpShot);
        gameStateMock.Object.Enemies.Should().Contain(enemyMock.Object);
    }

    #endregion

    #region Enemy Shot vs Player Collisions
    [Fact]
    public void GivenEnemyShotCollidesWithPlayer_WhenHandleCollisions_ThenPlayerTakesDamageAndShotRemoved()
    {
        // Given
        var damage = 10;
        var customShot = ShotFactory.CustomShot(
            new PositionalVector2(5, 5),
            new DirectionalVector2(1, 0),
            5,
            damage
        );

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns([]);
        gameStateMock.Setup(g => g.PlayerShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(g => g.EnemyShots).Returns([customShot]);

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, customShot)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(damage), Times.Once);
        gameStateMock.Object.EnemyShots.Should().BeEmpty();
        gameStateMock.Object.Player.IsDestroyed.Should().BeFalse();
    }

    [Fact]
    public void GivenEnemyShotDestroysPlayer_WhenHandleCollisions_ThenPlayerTakesDamageAndShotRemoved()
    {
        // Given
        var damage = 10;
        var customShot = ShotFactory.CustomShot(
            new PositionalVector2(5, 5),
            new DirectionalVector2(1, 0),
            5,
            damage
        );

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(true);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns([]);
        gameStateMock.Setup(g => g.PlayerShots).Returns([]);
        gameStateMock.Setup(g => g.EnemyShots).Returns([customShot]);

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, customShot)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(damage), Times.Once);
        gameStateMock.Object.EnemyShots.Should().BeEmpty();
        gameStateMock.Object.Player.IsDestroyed.Should().BeTrue();
    }

    [Fact]
    public void GivenEnemyShotDoesNotCollideWithPlayer_WhenHandleCollisions_ThenNoDamageTakenAndShotNotRemoved()
    {
        // Given
        var noOpShot = ShotFactory.CreateNoOpShot(new PositionalVector2(5, 5));

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns([]);
        gameStateMock.Setup(g => g.PlayerShots).Returns([]);
        gameStateMock.Setup(g => g.EnemyShots).Returns([noOpShot]);

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, noOpShot)).Returns(false);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(It.IsAny<int>()), Times.Never);
        gameStateMock.Object.EnemyShots.Should().Contain(noOpShot);
        gameStateMock.Object.Player.IsDestroyed.Should().BeFalse();
    }
    #endregion
}
