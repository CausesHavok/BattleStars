using System.Numerics;
using BattleStars.Core;
using FluentAssertions;
using BattleStars.Utility;
namespace BattleStars.Tests.Core;

public class BasicContextTest
{
    [Fact]
    public void TestPlayerDirection()
    {
        // Arrange
        var context = new BasicContext();
        var expectedDirection = new DirectionalVector2(1, 0);
        context.PlayerDirection = expectedDirection;

        // Act
        var actualDirection = context.PlayerDirection;

        // Assert
        expectedDirection.Should().Be(actualDirection);
    }

    [Fact]
    public void TestShooterPosition()
    {
        // Arrange
        var context = new BasicContext();
        var expectedPosition = new PositionalVector2(0, 1);
        context.ShooterPosition = expectedPosition;

        // Act
        var actualPosition = context.ShooterPosition;

        // Assert
        expectedPosition.Should().Be(actualPosition);
    }
}
