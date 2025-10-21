using Moq;
using FluentAssertions;
using BattleStars.Infrastructure.Factories;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;

namespace BattleStars.Tests.Infrastructure.Factories;

public class ControllerFactoryTests
{
    [Fact]
    public void GivenValidDependencies_WhenCreateGameControllerIsCalled_ThenReturnsGameControllerInstance()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(gs => gs.Validate());
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var collisionCheckerMock = new Mock<ICollisionChecker>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var contextMock = new Mock<IContext>();

        // When
        var gameController = ControllerFactory.CreateGameController(
            gameStateMock.Object,
            boundaryCheckerMock.Object,
            collisionCheckerMock.Object,
            inputHandlerMock.Object,
            contextMock.Object
        );

        // Then
        gameController.Should().NotBeNull();
        gameController.Should().BeOfType<GameController>();
    }

    [Fact]
    public void GivenNullGameState_WhenCreateGameControllerIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        IGameState gameState = null!;
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var collisionCheckerMock = new Mock<ICollisionChecker>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var contextMock = new Mock<IContext>();

        // When
        Action act = () => ControllerFactory.CreateGameController(
            gameState,
            boundaryCheckerMock.Object,
            collisionCheckerMock.Object,
            inputHandlerMock.Object,
            contextMock.Object
        );

        // Then
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("gameState");
    }

    [Fact]
    public void GivenNullBoundaryChecker_WhenCreateGameControllerIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        var boundaryChecker = null as IBoundaryChecker;
        var collisionCheckerMock = new Mock<ICollisionChecker>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var contextMock = new Mock<IContext>();

        // When
        Action act = () => ControllerFactory.CreateGameController(
            gameStateMock.Object,
            boundaryChecker!,
            collisionCheckerMock.Object,
            inputHandlerMock.Object,
            contextMock.Object
        );

        // Then
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("boundaryChecker");
    }

    [Fact]
    public void GivenNullCollisionChecker_WhenCreateGameControllerIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        ICollisionChecker collisionChecker = null!;
        var inputHandlerMock = new Mock<IInputHandler>();
        var contextMock = new Mock<IContext>();

        // When
        Action act = () => ControllerFactory.CreateGameController(
            gameStateMock.Object,
            boundaryCheckerMock.Object,
            collisionChecker,
            inputHandlerMock.Object,
            contextMock.Object
        );

        // Then
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("collisionChecker");
    }

    [Fact]
    public void GivenNullInputHandler_WhenCreateGameControllerIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var collisionCheckerMock = new Mock<ICollisionChecker>();
        IInputHandler inputHandler = null!;
        var contextMock = new Mock<IContext>();

        // When
        Action act = () => ControllerFactory.CreateGameController(
            gameStateMock.Object,
            boundaryCheckerMock.Object,
            collisionCheckerMock.Object,
            inputHandler,
            contextMock.Object
        );

        // Then
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("inputHandler");
    }

    [Fact]
    public void GivenNullContext_WhenCreateGameControllerIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var collisionCheckerMock = new Mock<ICollisionChecker>();
        var inputHandlerMock = new Mock<IInputHandler>();
        IContext context = null!;

        // When
        Action act = () => ControllerFactory.CreateGameController(
            gameStateMock.Object,
            boundaryCheckerMock.Object,
            collisionCheckerMock.Object,
            inputHandlerMock.Object,
            context
        );

        // Then
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("context");
    }
}