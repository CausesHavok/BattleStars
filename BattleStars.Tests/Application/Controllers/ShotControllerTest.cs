using Moq;
using FluentAssertions;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Controllers;

public class ShotControllerTest
{
    // Null tests are not needed as this class is a composite component of GameController
    // and the parameters are already validated in GameController or during Factory construction.

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
        gameStateMock.Setup(g => g.PlayerShots).Returns(ShotFactory.CreateEmptyShotList());
        gameStateMock.Setup(g => g.EnemyShots).Returns(ShotFactory.CreateEmptyShotList());
        var controller = new ShotController();

        // When
        var act = () => controller.UpdateShots(gameStateMock.Object);

        // Then
        act.Should().NotThrow();
    }
    #endregion
}