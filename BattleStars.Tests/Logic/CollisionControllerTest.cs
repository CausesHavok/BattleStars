using FluentAssertions;
using Moq;
using BattleStars.Core;
using BattleStars.Logic;
using BattleStars.Shots;

namespace BattleStars.Tests.Logic;

public class CollisionControllerTest
{
    #region Null Checks
    [Fact]
    public void GivenNullGameState_WhenHandleCollisions_ThenThrowsArgumentNullException()
    {
        // Given
        var controller = new CollisionController();

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
        var controller = new CollisionController();

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
        var controller = new CollisionController();

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
        var controller = new CollisionController();

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
        var controller = new CollisionController();

        // When
        var act = () => controller.HandleCollisions(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("EnemyShots");
    }

    #endregion

}