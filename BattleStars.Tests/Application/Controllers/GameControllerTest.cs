using Moq;
using FluentAssertions;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Factories;
using BattleStars.Domain.Entities;

namespace BattleStars.Tests.Application.Controllers;

public class GameControllerTest
{
    #region Construction Tests
    // Null tests are not needed as this class is created via the ControllerFactory
    // and the parameters are already validated in the Factory.

    [Fact]
    public void Constructor_ValidParameters_DoesNotThrow()
    {
        // Arrange
        Action act = () => ControllerFactory.CreateGameController(
            new Mock<IGameState>().Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            new Mock<IInputHandler>().Object);

        // Act / Assert
        act.Should().NotThrow();
    }

    #endregion

    #region RunFrame Tests
    [Fact]
    public void RunFrame_NullContext_ThrowsArgumentNullException()
    {
        // Arrange
        var gameController = ControllerFactory.CreateGameController(
            new Mock<IGameState>().Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            new Mock<IInputHandler>().Object);

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
        var inputHandlerMock = new Mock<IInputHandler>();

        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(true);

        var gameController = ControllerFactory.CreateGameController(
            new Mock<IGameState>().Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        // Act
        var result = gameController.RunFrame(contextMock.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void RunFrame_ValidContext_ProcessesFrameAndReturnsTrue()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(gs => gs.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(gs => gs.PlayerShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(gs => gs.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());

        var inputHandlerMock = new Mock<IInputHandler>();
        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(false);

        var gameController =  ControllerFactory.CreateGameController(
            gameStateMock.Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        // Act
        var result = gameController.RunFrame(contextMock.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void RunFrame_ValidContext_GameStateValidatedAfterProcessing()
    {
        // Arrange
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(gs => gs.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(gs => gs.PlayerShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(gs => gs.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(gs => gs.Validate()).Verifiable();
        var inputHandlerMock = new Mock<IInputHandler>();

        inputHandlerMock.Setup(ih => ih.ShouldExit()).Returns(false);
        

        var gameController =  ControllerFactory.CreateGameController(
            gameStateMock.Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            inputHandlerMock.Object);

        var contextMock = new Mock<IContext>();

        gameStateMock.Verify(gs => gs.Validate(), Times.Once); // Once during construction

        // Act
        gameController.RunFrame(contextMock.Object);

        // Assert
        gameStateMock.Verify(gs => gs.Validate(), Times.Exactly(2)); // Once during construction and once after processing
    }

    #endregion

    #region GetFrameSnapshot Tests
    [Fact]
    public void GetFrameSnapshot_ReturnsCorrectSnapshot()
    {
        // Arrange
        var playerMock = new Mock<IBattleStar>();
        var enemies = new List<IBattleStar> { new Mock<IBattleStar>().Object };
        var playerShots = new List<IShot> { ShotFactory.CreateNoOpShot() };
        var enemyShots = new List<IShot> { ShotFactory.CreateNoOpShot() };

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Player).Returns(playerMock.Object);
        gameStateMock.Setup(gs => gs.Enemies).Returns(enemies);
        gameStateMock.Setup(gs => gs.PlayerShots).Returns(playerShots);
        gameStateMock.Setup(gs => gs.EnemyShots).Returns(enemyShots);

        var gameController =  ControllerFactory.CreateGameController(
            gameStateMock.Object,
            new Mock<IBoundaryChecker>().Object,
            new Mock<ICollisionChecker>().Object,
            new Mock<IInputHandler>().Object
        );

        // Act
        var snapshot = gameController.GetFrameSnapshot();

        // Assert
        snapshot.Player.Should().Be(playerMock.Object);
        snapshot.Enemies.Should().BeEquivalentTo(enemies);
        snapshot.PlayerShots.Should().BeEquivalentTo(playerShots);
        snapshot.EnemyShots.Should().BeEquivalentTo(enemyShots);
        snapshot.ShouldContinue.Should().BeTrue();
    }

    #endregion
}