using Moq;
using FluentAssertions;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Controllers;

public class EnemyControllerTest
{

    // Null tests are not needed as this class is a composite component of GameController
    // and the parameters are already validated in GameController or during Factory construction.

    #region Behavior Tests
    [Fact]
    public void GivenEnemies_WhenUpdateEnemies_ThenAllEnemiesMoveAndShoot()
    {
        // Given
        var contextMock = new Mock<IContext>().Object;
        var enemyMock1 = new Mock<IBattleStar>();
        var enemyMock2 = new Mock<IBattleStar>();
        var noOpShot1 = ShotFactory.CreateNoOpShot();
        var noOpShot2 = ShotFactory.CreateNoOpShot();

        enemyMock1.Setup(e => e.Shoot(contextMock)).Returns([noOpShot1]);
        enemyMock2.Setup(e => e.Shoot(contextMock)).Returns([noOpShot2]);

        var enemies = new List<IBattleStar> { enemyMock1.Object, enemyMock2.Object };
        var enemyShots = ShotFactory.CreateEmptyShotList();

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Enemies).Returns(enemies);
        gameStateMock.Setup(g => g.EnemyShots).Returns(enemyShots);

        var controller = new EnemyController();

        // When
        controller.UpdateEnemies(contextMock, gameStateMock.Object);

        // Then
        enemyMock1.Verify(e => e.Move(contextMock), Times.Once);
        enemyMock2.Verify(e => e.Move(contextMock), Times.Once);
        enemyMock1.Verify(e => e.Shoot(contextMock), Times.Once);
        enemyMock2.Verify(e => e.Shoot(contextMock), Times.Once);
        enemyShots.Should().Contain([noOpShot1, noOpShot2]);
    }

    [Fact]
    public void GivenEnemiesShootNull_WhenUpdateEnemies_ThenNoShotsAreAdded()
    {
        // Given
        var contextMock = new Mock<IContext>().Object;
        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Shoot(contextMock)).Returns((IEnumerable<IShot>)null!);

        var enemies = new List<IBattleStar> { enemyMock.Object };
        var enemyShots = ShotFactory.CreateEmptyShotList();

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Enemies).Returns(enemies);
        gameStateMock.Setup(g => g.EnemyShots).Returns(enemyShots);

        var controller = new EnemyController();

        // When
        controller.UpdateEnemies(contextMock, gameStateMock.Object);

        // Then
        enemyMock.Verify(e => e.Move(contextMock), Times.Once);
        enemyMock.Verify(e => e.Shoot(contextMock), Times.Once);
        enemyShots.Should().BeEmpty();
    }
    #endregion
}