using Moq;
using FluentAssertions;
using BattleStars.Application.Controllers;
using BattleStars.Domain.Interfaces;

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
        var shot1 = new Mock<IShot>().Object;
        var shot2 = new Mock<IShot>().Object;

        enemyMock1.Setup(e => e.Shoot(contextMock)).Returns(new List<IShot> { shot1 });
        enemyMock2.Setup(e => e.Shoot(contextMock)).Returns(new List<IShot> { shot2 });

        var enemies = new List<IBattleStar> { enemyMock1.Object, enemyMock2.Object };
        var enemyShots = new List<IShot>();

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
        enemyShots.Should().Contain(new[] { shot1, shot2 });
    }

    [Fact]
    public void GivenEnemiesShootNull_WhenUpdateEnemies_ThenNoShotsAreAdded()
    {
        // Given
        var contextMock = new Mock<IContext>().Object;
        var enemyMock = new Mock<IBattleStar>();
        enemyMock.Setup(e => e.Shoot(contextMock)).Returns((IEnumerable<IShot>)null!);

        var enemies = new List<IBattleStar> { enemyMock.Object };
        var enemyShots = new List<IShot>();

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