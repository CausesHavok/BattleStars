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
    public void Constructor_NullGameState_ThrowsArgumentNullException()
    {
        // Arrange
        IGameState gameState = null!;
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameState,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("gameState");
    }

    [Fact]
    public void Constructor_NullPlayerController_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        IPlayerController playerController = null!;
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerController!,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("playerController");
    }

    [Fact]
    public void Constructor_NullEnemyController_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        IEnemyController enemyController = null!;
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyController!,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("enemyController");
    }

    [Fact]
    public void Constructor_NullShotController_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        IShotController shotController = null!;
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotController!,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("shotController");
    }

    [Fact]
    public void Constructor_NullBoundaryController_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        IBoundaryController boundaryController = null!;
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryController!,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("boundaryController");
    }

    [Fact]
    public void Constructor_NullCollisionController_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        ICollisionController collisionController = null!;
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionController!,
            inputHandlerMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("collisionController");
    }

    [Fact]
    public void Constructor_NullInputHandler_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        IInputHandler inputHandler = null!;

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandler!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("inputHandler");
    }

    [Fact]
    public void Constructor_ValidParameters_DoesNotThrow()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        // Act
        Action act = () => new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region RunFrame Tests
    [Fact]
    public void RunFrame_NullContext_ThrowsArgumentNullException()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        var gameController = new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        IContext context = null!;

        // Act
        Action act = () => gameController.RunFrame(context);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("context");
    }

    [Fact]
    public void RunFrame_InputHandlerIndicatesExit_ReturnsFalse()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(true);

        var gameController = new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        // Act
        var result = gameController.RunFrame(contextMock.Object);

        // Assert
        result.Should().BeFalse();
        playerControllerMock.Verify(pc => pc.UpdatePlayer(It.IsAny<IContext>(), It.IsAny<IInputHandler>(), It.IsAny<IGameState>()), Times.Never);
        enemyControllerMock.Verify(ec => ec.UpdateEnemies(It.IsAny<IContext>(), It.IsAny<IGameState>()), Times.Never);
        shotControllerMock.Verify(sc => sc.UpdateShots(It.IsAny<IGameState>()), Times.Never);
        boundaryControllerMock.Verify(bc => bc.EnforceBoundaries(It.IsAny<IGameState>()), Times.Never);
        collisionControllerMock.Verify(cc => cc.HandleCollisions(It.IsAny<IGameState>()), Times.Never);
    }

    [Fact]
    public void RunFrame_ValidContext_ProcessesFrameAndReturnsTrue()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(new Mock<IBattleStar>().Object);
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(false);

        var gameController = new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        // Act
        var result = gameController.RunFrame(contextMock.Object);

        // Assert
        result.Should().BeTrue();
        shotControllerMock.Verify(sc => sc.UpdateShots(gameStateMock.Object), Times.Once);
        playerControllerMock.Verify(pc => pc.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object), Times.Once);
        enemyControllerMock.Verify(ec => ec.UpdateEnemies(contextMock.Object, gameStateMock.Object), Times.Once);
        boundaryControllerMock.Verify(bc => bc.EnforceBoundaries(gameStateMock.Object), Times.Once);
        collisionControllerMock.Verify(cc => cc.HandleCollisions(gameStateMock.Object), Times.Once);
    }

    [Fact]
    public void RunFrame_ValidContext_GameStateValidatedAfterProcessing()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(new Mock<IBattleStar>().Object);
        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(false);
        gameStateMock.Setup(gs => gs.Validate()).Verifiable();

        var gameController = new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        // Act
        gameController.RunFrame(contextMock.Object);

        // Assert
        gameStateMock.Verify(gs => gs.Validate(), Times.Exactly(2)); // Once in constructor, once after processing
    }

    #endregion

    #region GetFrameSnapshot Tests
    [Fact]
    public void GetFrameSnapshot_ReturnsCorrectSnapshot()
    {
        // Arrange
        var playerMock = new Mock<IBattleStar>();
        var enemies = new List<IBattleStar> { new Mock<IBattleStar>().Object };
        var playerShots = new List<IShot> { new Mock<IShot>().Object };
        var enemyShots = new List<IShot> { new Mock<IShot>().Object };

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(playerMock.Object);
        gameStateMock.Setup(gs => gs.Enemies).Returns(enemies);
        gameStateMock.Setup(gs => gs.PlayerShots).Returns(playerShots);
        gameStateMock.Setup(gs => gs.EnemyShots).Returns(enemyShots);

        var playerControllerMock = new Mock<IPlayerController>();
        var enemyControllerMock = new Mock<IEnemyController>();
        var shotControllerMock = new Mock<IShotController>();
        var boundaryControllerMock = new Mock<IBoundaryController>();
        var collisionControllerMock = new Mock<ICollisionController>();
        var inputHandlerMock = new Mock<IInputHandler>();

        var gameController = new GameController(
            gameStateMock.Object,
            playerControllerMock.Object,
            enemyControllerMock.Object,
            shotControllerMock.Object,
            boundaryControllerMock.Object,
            collisionControllerMock.Object,
            inputHandlerMock.Object);

        // Act
        var snapshot = gameController.GetFrameSnapshot();

        // Assert
        snapshot.Player.Should().Be(playerMock.Object);
        snapshot.Enemies.Should().BeEquivalentTo(enemies);
        snapshot.PlayerShots.Should().BeEquivalentTo(playerShots);
        snapshot.EnemyShots.Should().BeEquivalentTo(enemyShots);
        snapshot.ShouldContinue.Should().BeTrue();
        gameStateMock.Verify(gs => gs.Validate(), Times.Exactly(2)); // Ensure Validate is called
    }

    #endregion
}