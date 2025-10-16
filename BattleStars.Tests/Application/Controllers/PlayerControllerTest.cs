using Moq;
using FluentAssertions;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Controllers;

public class PlayerControllerTest
{
    // Null tests are not needed as this class is a composite component of GameController
    // and the parameters are already validated in GameController or during Factory construction.

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
        var shotList = ShotFactory.CreateEmptyShotList();
        var expectedShots = new List<IShot> { ShotFactory.CreateNoOpShot() };

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
        var shotList = ShotFactory.CreateEmptyShotList();

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
        var shotList = ShotFactory.CreateEmptyShotList();

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
        var shotList = ShotFactory.CreateEmptyShotList();

        inputHandlerMock.Setup(i => i.GetMovement()).Returns(-DirectionalVector2.UnitX);
        inputHandlerMock.Setup(i => i.ShouldShoot()).Returns(true);
        playerMock.Setup(p => p.Shoot(contextMock.Object)).Returns(ShotFactory.CreateEmptyShotList());
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
        var shotList = ShotFactory.CreateEmptyShotList();
        var expectedShotsFirst = new List<IShot> { ShotFactory.CreateNoOpShot() };
        var expectedShotsSecond = new List<IShot> { ShotFactory.CreateNoOpShot(), ShotFactory.CreateNoOpShot() };

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