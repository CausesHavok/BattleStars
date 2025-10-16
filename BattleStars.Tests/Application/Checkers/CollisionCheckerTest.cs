using FluentAssertions;
using Moq;
using BattleStars.Application.Checkers;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Factories;

namespace BattleStars.Tests.Application.Checkers;

public class CollisionCheckerTest
{
    [Fact]
    public void GivenNullBattleStar_WhenCheckBattleStarShotCollision_ThenThrowsArgumentNullException()
    {
        var noOpShot = ShotFactory.CreateNoOpShot();
        var collisionChecker = new CollisionChecker();
        Action act = () => collisionChecker.CheckBattleStarShotCollision(null!, noOpShot);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenBattleStarAndShot_WhenContainsReturnsTrue_ThenReturnsTrue()
    {
        var battleStarMock = new Mock<IBattleStar>(MockBehavior.Strict);
        var shotPosition = new PositionalVector2(3, 4);
        var noOpShot = ShotFactory.CreateNoOpShot(shotPosition);

        battleStarMock.Setup(bs => bs.Contains(shotPosition)).Returns(true);

        var collisionChecker = new CollisionChecker();
        var result = collisionChecker.CheckBattleStarShotCollision(battleStarMock.Object, noOpShot);

        result.Should().BeTrue();
        battleStarMock.Verify(bs => bs.Contains(shotPosition), Times.Once);
    }

    [Fact]
    public void GivenBattleStarAndShot_WhenContainsReturnsFalse_ThenReturnsFalse()
    {
        var battleStarMock = new Mock<IBattleStar>(MockBehavior.Strict);
        var noOpShot = ShotFactory.CreateNoOpShot();
        battleStarMock.Setup(bs => bs.Contains(It.IsAny<PositionalVector2>())).Returns(false);

        var collisionChecker = new CollisionChecker();
        var result = collisionChecker.CheckBattleStarShotCollision(battleStarMock.Object, noOpShot);

        result.Should().BeFalse();
        battleStarMock.Verify(bs => bs.Contains(It.IsAny<PositionalVector2>()), Times.Once);
    }

}

