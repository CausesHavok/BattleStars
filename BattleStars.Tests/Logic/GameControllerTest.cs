using Moq;
using FluentAssertions;
using BattleStars.Core;
using BattleStars.Logic;
using BattleStars.Shots;
using BattleStars.Utility;

namespace BattleStars.Tests.Logic;

public class GameControllerTest
{
    #region Construction Tests
    [Fact]
    public void GivenNullGameState_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var inputHandler = new Mock<IInputHandler>().Object;
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;
        Action act = () => new GameController(null!, inputHandler, boundaryChecker);
        act.Should().Throw<ArgumentNullException>().WithMessage("*gameState*");
    }

    [Fact]
    public void GivenNullInputHandler_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;
        Action act = () => new GameController(gameState.Object, null!, boundaryChecker);
        act.Should().Throw<ArgumentNullException>().WithMessage("*inputHandler*");
    }

    [Fact]
    public void GivenNullBoundaryChecker_WhenConstructed_ThenThrowsArgumentNullException()
    {
        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var inputHandler = new Mock<IInputHandler>().Object;
        Action act = () => new GameController(gameState.Object, inputHandler, null!);
        act.Should().Throw<ArgumentNullException>().WithMessage("*boundaryChecker*");
    }

    [Fact]
    public void GivenNonNullInput_WhenConstructed_ThenDoesNotThrow()
    {
        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var inputHandler = new Mock<IInputHandler>().Object;
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;
        Action act = () => new GameController(gameState.Object, inputHandler, boundaryChecker);
        act.Should().NotThrow();
    }
    #endregion

    #region RunFrame Tests
    [Fact]
    public void GivenShouldExit_WhenRunFrameIsCalled_ThenShouldReturnFalse()
    {
        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldExit()).Returns(true);
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;
        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context).Should().BeFalse();
    }

    [Fact]
    public void GivenShouldNotExit_WhenRunFrameIsCalled_ThenShouldReturnTrue()
    {
        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldExit()).Returns(false);
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context).Should().BeTrue();
    }

    [Fact]
    public void GivenPlayerIsDestroyed_WhenGetFrameSnapshotIsCalled_ThenShouldContinueIsFalse()
    {

        var inputHandler = new Mock<IInputHandler>().Object;
        var boundaryChecker = new Mock<IBoundaryChecker>().Object;

        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(true);

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var controller = new GameController(gameState.Object, inputHandler, boundaryChecker);

        var snapshot = controller.GetFrameSnapshot();
        snapshot.ShouldContinue.Should().BeFalse();
    }

    [Fact]
    public void GetFrameSnapshot_ShouldReturnExpectedValues()
    {
        var gameState = new Mock<IGameState>();
        var playerShot = new Mock<IShot>().Object;
        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns([playerShot]);

        var enemyShot = new Mock<IShot>().Object;
        var enemy = new Mock<IBattleStar>();
        enemy.Setup(e => e.IsDestroyed).Returns(false);
        enemy.Setup(e => e.Shoot(It.IsAny<IContext>())).Returns([enemyShot]);

        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar> { enemy.Object });
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(true);

        var boundaryChecker = new Mock<IBoundaryChecker>().Object;

        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker);

        // Simulate adding shots
        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // This will add shots via Shoot

        var snapshot = controller.GetFrameSnapshot();
        snapshot.PlayerShots.Should().Contain(playerShot);
        snapshot.EnemyShots.Should().Contain(enemyShot);
        snapshot.Player.Should().Be(player.Object);
        snapshot.Enemies.Should().Contain(enemy.Object);
        snapshot.ShouldContinue.Should().BeTrue();
    }

    [Fact]
    public void GivenMultipleFrames_WhenRunFrameIsCalled_ThenShotsAreAccumulated()
    {
        var playerShot1 = new Mock<IShot>().Object;
        var playerShot2 = new Mock<IShot>().Object;
        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        var shootCallCount = 0;
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns(() =>
        {
            shootCallCount++;
            return shootCallCount == 1 ? [playerShot1] : [playerShot2];
        });

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(true);

        var boundaryChecker = new Mock<IBoundaryChecker>().Object;

        List<IBattleStar> enemies = [];

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns(enemies);
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // First frame
        controller.RunFrame(context); // Second frame

        var snapshot = controller.GetFrameSnapshot();
        snapshot.PlayerShots.Should().Contain([playerShot1, playerShot2]);
    }

    [Fact]
    public void GivenNoShots_WhenGetFrameSnapshotIsCalled_ThenShotsAreEmpty()
    {
        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns([]);

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(false);

        var boundaryChecker = new Mock<IBoundaryChecker>().Object;

        List<IBattleStar> enemies = new List<IBattleStar>();
        var GameState = new Mock<IGameState>();
        GameState.Setup(g => g.Player).Returns(player.Object);
        GameState.Setup(g => g.Enemies).Returns(enemies);
        GameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        GameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var controller = new GameController(GameState.Object, inputHandler.Object, boundaryChecker);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // This will not add any shots

        var snapshot = controller.GetFrameSnapshot();
        snapshot.PlayerShots.Should().BeEmpty();
        snapshot.EnemyShots.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShots_WhenOutsideBoundary_ThenShotsAreRemoved()
    {
        var playerShotInside = new Mock<IShot>();
        var playerShotInsidePosition = new PositionalVector2(5f, 5f);
        playerShotInside.Setup(s => s.Position).Returns(playerShotInsidePosition);
        var playerShotOutside = new Mock<IShot>();
        var playerShotOutsidePosition = new PositionalVector2(15f, 15f);
        playerShotOutside.Setup(s => s.Position).Returns(playerShotOutsidePosition);

        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        var shootCallCount = 0;
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns(() =>
        {
            shootCallCount++;
            return shootCallCount == 1 ? [playerShotInside.Object] : [playerShotOutside.Object];
        });

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(true);

        var boundaryChecker = new Mock<IBoundaryChecker>();
        boundaryChecker.Setup(b => b.IsOutsideXBounds(5f)).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideXBounds(15f)).Returns(true);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(5f)).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(15f)).Returns(false);

        List<IBattleStar> enemies = [];
        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns(enemies);
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker.Object);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // First frame
        controller.RunFrame(context); // Second frame

        var snapshot = controller.GetFrameSnapshot();
        snapshot.PlayerShots.Should().Contain(playerShotInside.Object);
        snapshot.PlayerShots.Should().NotContain(playerShotOutside.Object);
    }

    [Fact]
    public void GivenEnemyShots_WhenOutsideBoundary_ThenShotsAreRemoved()
    {
        var enemyShotInside = new Mock<IShot>();
        var enemyShotInsidePosition = new PositionalVector2(5f, 5f);
        enemyShotInside.Setup(s => s.Position).Returns(enemyShotInsidePosition);
        var enemyShotOutside = new Mock<IShot>();
        var enemyShotOutsidePosition = new PositionalVector2(15f, 15f);
        enemyShotOutside.Setup(s => s.Position).Returns(enemyShotOutsidePosition);

        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns([]);

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(false);

        var boundaryChecker = new Mock<IBoundaryChecker>();
        boundaryChecker.Setup(b => b.IsOutsideXBounds(5f)).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideXBounds(15f)).Returns(true);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(5f)).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(15f)).Returns(false);

        var enemy = new Mock<IBattleStar>();
        enemy.Setup(e => e.IsDestroyed).Returns(false);
        var shootCallCount = 0;
        enemy.Setup(e => e.Shoot(It.IsAny<IContext>())).Returns(() =>
        {
            shootCallCount++;
            return shootCallCount == 1 ? [enemyShotInside.Object] : [enemyShotOutside.Object];
        });

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns([enemy.Object]);
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker.Object);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // First frame
        controller.RunFrame(context); // Second frame

        var snapshot = controller.GetFrameSnapshot();
        snapshot.EnemyShots.Should().Contain(enemyShotInside.Object);
        snapshot.EnemyShots.Should().NotContain(enemyShotOutside.Object);
    }

    [Fact]
    public void GivenCollidingShots_WhenGetFrameSnapshotIsCalled_ThenShotsAreRemoved()
    {
        var playerShot = new Mock<IShot>();
        var shotPosition = new PositionalVector2(5f, 5f);
        playerShot.Setup(s => s.Position).Returns(shotPosition);

        var enemyShot = new Mock<IShot>();
        enemyShot.Setup(s => s.Position).Returns(shotPosition);

        var player = new Mock<IBattleStar>();
        player.Setup(p => p.IsDestroyed).Returns(false);
        player.Setup(p => p.Shoot(It.IsAny<IContext>())).Returns([playerShot.Object]);
        player.Setup(p => p.Contains(It.IsAny<PositionalVector2>())).Returns(true);

        var inputHandler = new Mock<IInputHandler>();
        inputHandler.Setup(i => i.ShouldShoot()).Returns(true);

        var boundaryChecker = new Mock<IBoundaryChecker>();
        boundaryChecker.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns(false);

        var enemy = new Mock<IBattleStar>();
        enemy.Setup(e => e.IsDestroyed).Returns(false);
        enemy.Setup(e => e.Shoot(It.IsAny<IContext>())).Returns([enemyShot.Object]);
        enemy.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns([enemy.Object]);
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker.Object);

        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // This will add shots via Shoot

        var snapshot = controller.GetFrameSnapshot();
        snapshot.PlayerShots.Should().BeEmpty();
        snapshot.EnemyShots.Should().BeEmpty();
    }

    [Fact]
    public void GivenEnemy_WhenShotCollides_ThenEnemyDiesAndIsRemoved()
    {
        var inputHandler = new Mock<IInputHandler>();
        var boundaryChecker = new Mock<IBoundaryChecker>();
        boundaryChecker.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns(false);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns(false);

        var player = new Mock<IBattleStar>();
        var playershot = new Mock<IShot>();
        var enemy = new Mock<IBattleStar>();
        enemy.Setup(e => e.IsDestroyed).Returns(true);
        enemy.Setup(e => e.Contains(It.IsAny<PositionalVector2>())).Returns(true);

        var gameState = new Mock<IGameState>();
        gameState.Setup(g => g.Player).Returns(player.Object);
        gameState.Setup(g => g.Enemies).Returns(new List<IBattleStar> { enemy.Object });
        gameState.Setup(g => g.PlayerShots).Returns(new List<IShot> { playershot.Object });
        gameState.Setup(g => g.EnemyShots).Returns(new List<IShot>());

        var controller = new GameController(gameState.Object, inputHandler.Object, boundaryChecker.Object);
        var context = new Mock<IContext>().Object;
        controller.RunFrame(context); // This will process the collision

        var snapshot = controller.GetFrameSnapshot();
        snapshot.Enemies.Should().NotContain(enemy.Object);


    }


    #endregion
}