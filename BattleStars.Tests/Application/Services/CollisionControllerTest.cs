using FluentAssertions;
using Moq;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Application.Services;

namespace BattleStars.Tests.Application.Services;

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
        act.Should().Throw<ArgumentNullException>().WithParameterName("Player");
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
        act.Should().Throw<ArgumentNullException>().WithParameterName("Enemies");
    }

    [Fact]
    public void GivenNullPlayerShots_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns((List<IShot>)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("PlayerShots");
    }

    [Fact]
    public void GivenNullEnemyShots_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns((List<IShot>)null!);
        var collisionChecker = new Mock<ICollisionChecker>();
        var controller = new CollisionController(collisionChecker.Object);

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("EnemyShots");
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
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();
        enemyMock.Setup(e => e.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar> { enemyMock.Object });
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot> { shotMock.Object });
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, shotMock.Object)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(10), Times.Once);
        gameStateMock.Object.PlayerShots.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShotDestroysEnemy_WhenHandleCollisions_ThenEnemyAndShotRemoved()
    {
        // Given
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();
        enemyMock.Setup(e => e.IsDestroyed).Returns(true);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar> { enemyMock.Object });
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot> { shotMock.Object });
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, shotMock.Object)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(10), Times.Once);
        gameStateMock.Object.PlayerShots.Should().BeEmpty();
        gameStateMock.Object.Enemies.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShotDoesNotCollideWithEnemy_WhenHandleCollisions_ThenNoDamageAndShotNotRemoved()
    {
        // Given
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(false);
        enemyMock.Setup(e => e.TakeDamage(It.IsAny<int>())).Verifiable();

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar> { enemyMock.Object });
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot> { shotMock.Object });
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(enemyMock.Object, shotMock.Object)).Returns(false);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.TakeDamage(It.IsAny<int>()), Times.Never);
        gameStateMock.Object.PlayerShots.Should().Contain(shotMock.Object);
        gameStateMock.Object.Enemies.Should().Contain(enemyMock.Object);
    }

    #endregion

    #region Enemy Shot vs Player Collisions
    [Fact]
    public void GivenEnemyShotCollidesWithPlayer_WhenHandleCollisions_ThenPlayerTakesDamageAndShotRemoved()
    {
        // Given
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot> { shotMock.Object });

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, shotMock.Object)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(10), Times.Once);
        gameStateMock.Object.EnemyShots.Should().BeEmpty();
        gameStateMock.Object.Player.IsDestroyed.Should().BeFalse();
    }

    [Fact]
    public void GivenEnemyShotDestroysPlayer_WhenHandleCollisions_ThenPlayerTakesDamageAndShotRemoved()
    {
        // Given
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(true);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot> { shotMock.Object });

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, shotMock.Object)).Returns(true);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(10), Times.Once);
        gameStateMock.Object.EnemyShots.Should().BeEmpty();
        gameStateMock.Object.Player.IsDestroyed.Should().BeTrue();
    }

    [Fact]
    public void GivenEnemyShotDoesNotCollideWithPlayer_WhenHandleCollisions_ThenNoDamageTakenAndShotNotRemoved()
    {
        // Given
        var shotMock = new Mock<IShot>();
        shotMock.Setup(s => s.Position).Returns(new PositionalVector2(5, 5));
        shotMock.Setup(s => s.Damage).Returns(10);

        var playerMock = new Mock<IBattleStar>();
        playerMock.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);
        playerMock.Setup(p => p.TakeDamage(It.IsAny<int>())).Verifiable();
        playerMock.Setup(p => p.IsDestroyed).Returns(false);

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot> { shotMock.Object });

        var collisionCheckerMock = new Mock<ICollisionChecker>();
        collisionCheckerMock.Setup(c => c.CheckBattleStarShotCollision(playerMock.Object, shotMock.Object)).Returns(false);

        var controller = new CollisionController(collisionCheckerMock.Object);

        // When
        controller.HandleCollisions(gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.TakeDamage(It.IsAny<int>()), Times.Never);
        gameStateMock.Object.EnemyShots.Should().Contain(shotMock.Object);
        gameStateMock.Object.Player.IsDestroyed.Should().BeFalse();
    }
    #endregion
}
