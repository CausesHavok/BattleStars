using FluentAssertions;
using Moq;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.Application.Controllers;

public class PlayerControllerTest
{
    #region Null Checks

    [Fact]
    public void GivenNullContext_WhenUpdatePlayer_ThenThrowsArgumentNullException()
    {
        // Given
        var inputHandler = new Mock<IInputHandler>().Object;
        var gameState = new Mock<IGameState>().Object;
        var controller = new PlayerController();

        // When
        var act = () => controller.UpdatePlayer(null!, inputHandler, gameState);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("context");
    }

    [Fact]
    public void GivenNullInputHandler_WhenUpdatePlayer_ThenThrowsArgumentNullException()
    {
        // Given
        var contextMock = new Mock<IContext>().Object;
        IInputHandler inputHandler = null!;
        var gameStateMock = new Mock<IGameState>().Object;
        var controller = new PlayerController();

        // When
        var act = () => controller.UpdatePlayer(contextMock, inputHandler, gameStateMock);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("inputHandler");
    }

    [Fact]
    public void GivenNullGameState_WhenUpdatePlayer_ThenThrowsArgumentNullException()
    {
        // Given
        var contextMock = new Mock<IContext>().Object;
        var inputHandlerMock = new Mock<IInputHandler>().Object;
        IGameState gameState = null!;
        var controller = new PlayerController();

        // When
        var act = () => controller.UpdatePlayer(contextMock, inputHandlerMock, gameState);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState");
    }

    [Fact]
    public void GivenPlayerIsNull_WhenUpdatePlayer_ThenThrowsArgumentNullException()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var shotList = new List<IShot>();

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(DirectionalVector2.UnitX);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(true);
        gameStateMock.Setup(g => g.Player).Returns((IBattleStar?)null!);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        var act = () => controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("Player");
    }

    #endregion

    #region Movement Tests

    [Fact]
    public void GivenValidInput_WhenUpdatePlayer_ThenUpdatesPlayerDirectionAndMovesPlayer()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();

        contextMock.SetupProperty(c => c.PlayerDirection);
        inputHandlerMock.Setup(i => i.GetMovement()).Returns(DirectionalVector2.UnitX);
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        contextMock.Object.PlayerDirection.Should().Be(DirectionalVector2.UnitX);
        playerMock.Verify(p => p.Move(contextMock.Object), Times.Once);
    }

    #endregion

    #region Shooting Tests

    [Fact]
    public void GivenShouldShootTrue_WhenUpdatePlayer_ThenPlayerShootsAndShotsAreAdded()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();
        var shotList = new List<IShot>();
        var expectedShots = new List<IShot> { new Mock<IShot>().Object };

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(DirectionalVector2.UnitY);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(true);
        playerMock.Setup(p => p.Shoot(contextMock.Object)).Returns(expectedShots);
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        shotList.Should().Contain(expectedShots);
    }

    [Fact]
    public void GivenShouldShootFalse_WhenUpdatePlayer_ThenPlayerDoesNotShoot()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();
        var shotList = new List<IShot>();

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(-DirectionalVector2.UnitY);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(false);
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        playerMock.Verify(p => p.Shoot(It.IsAny<IContext>()), Times.Never);
        shotList.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShootsNull_WhenUpdatePlayer_ThenNoShotsAreAdded()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();
        var shotList = new List<IShot>();

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(DirectionalVector2.UnitX);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(true);
        playerMock.Setup(p => p.Shoot(contextMock.Object)).Returns((IEnumerable<IShot>?)null!);
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        shotList.Should().BeEmpty();
    }

    [Fact]
    public void GivenPlayerShootsEmpty_WhenUpdatePlayer_ThenNoShotsAreAdded()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();
        var shotList = new List<IShot>();

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(-DirectionalVector2.UnitX);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(true);
        playerMock.Setup(p => p.Shoot(contextMock.Object)).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        shotList.Should().BeEmpty();
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void GivenMultipleUpdates_WhenUpdatePlayer_ThenStateIsConsistent()
    {
        // Given
        var contextMock = new Mock<IContext>();
        var inputHandlerMock = new Mock<IInputHandler>();
        var gameStateMock = new Mock<IGameState>();
        var playerMock = new Mock<IBattleStar>();
        var shotList = new List<IShot>();
        var expectedShotsFirst = new List<IShot> { new Mock<IShot>().Object };
        var expectedShotsSecond = new List<IShot> { new Mock<IShot>().Object, new Mock<IShot>().Object };

        contextMock.SetupProperty(c => c.PlayerDirection);
        inputHandlerMock.SetupSequence(i => i.GetMovement())
            .Returns(DirectionalVector2.UnitX)
            .Returns(DirectionalVector2.UnitY);
        inputHandlerMock.SetupSequence(i => i.ShouldShoot())
            .Returns(true)
            .Returns(true);
        playerMock.SetupSequence(p => p.Shoot(contextMock.Object))
            .Returns(expectedShotsFirst)
            .Returns(expectedShotsSecond);
        gameStateMock.Setup(g => g.Player).Returns(playerMock.Object);
        gameStateMock.Setup(g => g.PlayerShots).Returns(shotList);

        var controller = new PlayerController();

        // When
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);
        controller.UpdatePlayer(contextMock.Object, inputHandlerMock.Object, gameStateMock.Object);

        // Then
        contextMock.Object.PlayerDirection.Should().Be(DirectionalVector2.UnitY);
        playerMock.Verify(p => p.Move(contextMock.Object), Times.Exactly(2));
        shotList.Should().Contain(expectedShotsFirst);
        shotList.Should().Contain(expectedShotsSecond);
    }

    #endregion

}