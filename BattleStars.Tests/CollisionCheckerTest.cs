using System.Numerics;
using FluentAssertions;
using Moq;
using BattleStars.Logic;
using BattleStars.Core;
using BattleStars.Shapes;
using BattleStars.Shots;

namespace BattleStars.Tests.Logic;

public class CollisionCheckerTest
{
    [Fact]
    public void GivenNullEntity_WhenCheckEntityShotCollision_ThenThrowsArgumentNullException()
    {
        var shotMock = new Mock<IShot>().Object;
        Action act = () => CollisionChecker.CheckEntityShotCollision(null!, shotMock);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShot_WhenCheckEntityShotCollision_ThenThrowsArgumentNullException()
    {
        var entityMock = new Mock<Entity>(MockBehavior.Strict).Object;
        Action act = () => CollisionChecker.CheckEntityShotCollision(entityMock, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenEntityAndShot_WhenShapeContainsAdjustedPosition_ThenReturnsTrue()
    {
        var entityMock = new Mock<Entity>(MockBehavior.Strict);
        var shapeMock = new Mock<IShape>();
        var shotMock = new Mock<IShot>();

        var entityPosition = new Vector2(5, 5);
        var shotPosition = new Vector2(7, 8);
        var adjustedPosition = shotPosition - entityPosition;

        entityMock.Setup(e => e.Position).Returns(entityPosition);
        entityMock.Setup(e => e.Shape).Returns(shapeMock.Object);
        shotMock.Setup(s => s.Position).Returns(shotPosition);
        shapeMock.Setup(s => s.Contains(adjustedPosition)).Returns(true);

        var result = CollisionChecker.CheckEntityShotCollision(entityMock.Object, shotMock.Object);

        result.Should().BeTrue();
        shapeMock.Verify(s => s.Contains(adjustedPosition), Times.Once);
    }

    [Fact]
    public void GivenEntityAndShot_WhenShapeDoesNotContainAdjustedPosition_ThenReturnsFalse()
    {
        var entityMock = new Mock<Entity>(MockBehavior.Strict);
        var shapeMock = new Mock<IShape>();
        var shotMock = new Mock<IShot>();

        var entityPosition = new Vector2(1, 1);
        var shotPosition = new Vector2(10, 10);
        var adjustedPosition = shotPosition - entityPosition;

        entityMock.Setup(e => e.Position).Returns(entityPosition);
        entityMock.Setup(e => e.Shape).Returns(shapeMock.Object);
        shotMock.Setup(s => s.Position).Returns(shotPosition);
        shapeMock.Setup(s => s.Contains(adjustedPosition)).Returns(false);

        var result = CollisionChecker.CheckEntityShotCollision(entityMock.Object, shotMock.Object);

        result.Should().BeFalse();
        shapeMock.Verify(s => s.Contains(adjustedPosition), Times.Once);
    }

    [Fact]
    public void GivenNullBattleStar_WhenCheckBattleStarShotCollision_ThenThrowsArgumentNullException()
    {
        var shotMock = new Mock<IShot>().Object;
        Action act = () => CollisionChecker.CheckBattleStarShotCollision(null!, shotMock);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShot_WhenCheckBattleStarShotCollision_ThenThrowsArgumentNullException()
    {
        var battleStarMock = new Mock<BattleStar>(MockBehavior.Strict, null!, null!, null!, null!, null!).Object;
        Action act = () => CollisionChecker.CheckBattleStarShotCollision(battleStarMock, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenBattleStarAndShot_WhenContainsReturnsTrue_ThenReturnsTrue()
    {
        var battleStarMock = new Mock<BattleStar>(MockBehavior.Strict, null!, null!, null!, null!, null!);
        var shotMock = new Mock<IShot>();

        var shotPosition = new Vector2(3, 4);
        shotMock.Setup(s => s.Position).Returns(shotPosition);
        battleStarMock.Setup(bs => bs.Contains(shotPosition)).Returns(true);

        var result = CollisionChecker.CheckBattleStarShotCollision(battleStarMock.Object, shotMock.Object);

        result.Should().BeTrue();
        battleStarMock.Verify(bs => bs.Contains(shotPosition), Times.Once);
    }

    [Fact]
    public void GivenBattleStarAndShot_WhenContainsReturnsFalse_ThenReturnsFalse()
    {
        var battleStarMock = new Mock<BattleStar>(MockBehavior.Strict, null!, null!, null!, null!, null!);
        var shotMock = new Mock<IShot>();

        var shotPosition = new Vector2(10, 10);
        shotMock.Setup(s => s.Position).Returns(shotPosition);
        battleStarMock.Setup(bs => bs.Contains(shotPosition)).Returns(false);

        var result = CollisionChecker.CheckBattleStarShotCollision(battleStarMock.Object, shotMock.Object);

        result.Should().BeFalse();
        battleStarMock.Verify(bs => bs.Contains(shotPosition), Times.Once);
    }
}