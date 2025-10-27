using FluentAssertions;
using Moq;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Application.Controllers;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Controllers;

public class BoundaryControllerTest
{
    #region Null Checks
    [Fact]
    public void GivenNullBoundaryChecker_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Given
        IBoundaryChecker boundaryChecker = null!;

        // When
        var act = () => new BoundaryController(boundaryChecker);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("boundaryChecker");
    }

    [Fact]
    public void GivenNullGameState_WhenEnforceBoundaries_ThenThrowsArgumentNullException()
    {
        // Given
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var controller = new BoundaryController(boundaryCheckerMock.Object);

        // When
        var act = () => controller.EnforceBoundaries(null!);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState");
    }

    [Fact]
    public void GivenNullPlayer_WhenEnforceBoundaries_ThenThrowsArgumentNullException()
    {
        // Given
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns((IBattleStar)null!);
        var controller = new BoundaryController(boundaryCheckerMock.Object);

        // When
        var act = () => controller.EnforceBoundaries(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.Player");
    }

    [Fact]
    public void GivenNullEnemies_WhenEnforceBoundaries_ThenThrowsArgumentNullException()
    {
        // Given
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns((List<IBattleStar>)null!);
        var controller = new BoundaryController(boundaryCheckerMock.Object);

        // When
        var act = () => controller.EnforceBoundaries(gameStateMock.Object);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("gameState.Enemies");
    }

    #endregion

    #region Behavior Tests

    [Fact]
    public void GivenShotsOutsideBounds_WhenEnforceBoundaries_ThenShotsAreRemoved()
    {
        // Given
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        boundaryCheckerMock.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns(true);
        boundaryCheckerMock.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns(false);

        var noOpShot1 = ShotFactory.CreateNoOpShot(new PositionalVector2(1000, 50));
        var noOpShot2 = ShotFactory.CreateNoOpShot(new PositionalVector2(-100, 50));

        var playerShots = new List<IShot> { noOpShot1, noOpShot2 };
        var enemyShots = new List<IShot> { noOpShot1, noOpShot2 };

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(playerShots);
        gameStateMock.Setup(g => g.EnemyShots).Returns(enemyShots);

        var controller = new BoundaryController(boundaryCheckerMock.Object);

        // When
        controller.EnforceBoundaries(gameStateMock.Object);

        // Then
        playerShots.Should().BeEmpty();
        enemyShots.Should().BeEmpty();
    }

    [Fact]
    public void GivenShotsInsideBounds_WhenEnforceBoundaries_ThenShotsAreNotRemoved()
    {
        // Given
        var boundaryCheckerMock = new Mock<IBoundaryChecker>();
        boundaryCheckerMock.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns(false);
        boundaryCheckerMock.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns(false);

        var noOpShot1 = ShotFactory.CreateNoOpShot(new PositionalVector2(100, 50));
        var noOpShot2 = ShotFactory.CreateNoOpShot(new PositionalVector2(200, 150));

        var playerShots = new List<IShot> { noOpShot1, noOpShot2 };
        var enemyShots = new List<IShot> { noOpShot1, noOpShot2 };

        var gameStateMock = new Mock<IGameState>();
        gameStateMock.Setup(g => g.Player).Returns(new Mock<IBattleStar>().Object);
        gameStateMock.Setup(g => g.Enemies).Returns(new List<IBattleStar>());
        gameStateMock.Setup(g => g.PlayerShots).Returns(playerShots);
        gameStateMock.Setup(g => g.EnemyShots).Returns(enemyShots);

        var controller = new BoundaryController(boundaryCheckerMock.Object);

        // When
        controller.EnforceBoundaries(gameStateMock.Object);

        // Then
        playerShots.Should().HaveCount(2);
        enemyShots.Should().HaveCount(2);
    }
    #endregion
}