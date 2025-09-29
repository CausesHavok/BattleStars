using FluentAssertions;
using Moq;
using BattleStars.Logic;
using BattleStars.Shots;

namespace BattleStars.Tests.Logic;

public class ShotControllerTest
{
    #region Null Checks
    [Fact]
    public void GivenNullGameState_WhenUpdateShots_ThenThrowsArgumentNullException()
    {
        // Given
        var controller = new ShotController();

        // When
        var act = () => controller.UpdateShots(null!);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState");
    }

    [Fact]
    public void GivenNullPlayerShots_WhenUpdateShots_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.PlayerShots).Returns((List<IShot>)null!);
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var controller = new ShotController();

        // When
        var act = () => controller.UpdateShots(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("PlayerShots");
    }

    [Fact]
    public void GivenNullEnemyShots_WhenUpdateShots_ThenThrowsArgumentNullException()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns((List<IShot>)null!);
        var controller = new ShotController();

        // When
        var act = () => controller.UpdateShots(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("EnemyShots");
    }

    #endregion

    #region Behavior Tests
    [Fact]
    public void GivenShots_WhenUpdateShots_ThenAllShotsAreUpdated()
    {
        // Given
        var playerShotMock1 = new Mock<IShot>();
        var playerShotMock2 = new Mock<IShot>();
        var enemyShotMock1 = new Mock<IShot>();
        var enemyShotMock2 = new Mock<IShot>();

        var playerShots = new List<IShot> { playerShotMock1.Object, playerShotMock2.Object };
        var enemyShots = new List<IShot> { enemyShotMock1.Object, enemyShotMock2.Object };

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.PlayerShots).Returns(playerShots);
        gameStateMock.Setup(g => g.EnemyShots).Returns(enemyShots);

        var controller = new ShotController();

        // When
        controller.UpdateShots(gameStateMock.Object);

        // Then
        playerShotMock1.Verify(s => s.Update(), Times.Once);
        playerShotMock2.Verify(s => s.Update(), Times.Once);
        enemyShotMock1.Verify(s => s.Update(), Times.Once);
        enemyShotMock2.Verify(s => s.Update(), Times.Once);
    }

    [Fact]
    public void GivenEmptyShots_WhenUpdateShots_ThenNoShotIsUpdated()
    {
        // Given
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.PlayerShots).Returns(new List<IShot>());
        gameStateMock.Setup(g => g.EnemyShots).Returns(new List<IShot>());
        var controller = new ShotController();

        // When
        var act = () => controller.UpdateShots(gameStateMock.Object);

        // Then
        act.Should().NotThrow();
    }
    #endregion
}