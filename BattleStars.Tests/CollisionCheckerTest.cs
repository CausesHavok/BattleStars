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