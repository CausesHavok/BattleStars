using FluentAssertions;
using Moq;
using BattleStars.Domain.Entities;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.Domain.Entities;

public class BasicMovableTest
{
    #region Constructor Tests
    // 4. Throws if speed is NaN or Infinity.
    // 5. Throws if speed is negative or zero.
    // 6. Does not throw for valid inputs.
    // 7. Sets initial position correctly.
    // 8. Sets direction and speed correctly (indirectly tested via movement).

    [Theory] // 4
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidSpeed_WhenConstructed_ThenThrowsArgumentException(float speed)
    {
        var initialPosition = PositionalVector2.Zero;
        var direction = DirectionalVector2.UnitY;

        Action act = () => new BasicMovable(initialPosition, direction, speed);

        act.Should().Throw<ArgumentException>();
    }

    [Theory] // 5
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenNegativeOrZeroSpeed_WhenConstructed_ThenThrowsArgumentException(float speed)
    {
        var initialPosition = PositionalVector2.Zero;
        var direction = DirectionalVector2.UnitY;

        Action act = () => new BasicMovable(initialPosition, direction, speed);

        act.Should().Throw<ArgumentException>();
    }

    [Fact] // 6 & 7
    public void GivenValidInputs_WhenConstructed_ThenDoesNotThrowAndSetsInitialPosition()
    {
        var initialPosition = new PositionalVector2(1, 2);
        var direction = DirectionalVector2.UnitY;
        var speed = 1f;

        var movable = new BasicMovable(initialPosition, direction, speed);

        movable.Position.Should().Be(initialPosition);
    }
    #endregion

    #region Move Method Tests
    // 10. Updates position as expected for valid context.
    // 11. Multiple moves accumulate position correctly.
    // 12. Position, direction, speed, and context are not mutated by Move.
    // 13. Zero direction results in no movement.
    // 16. Edge case positions (large/small values) move as expected.

    [Fact] // 10 (and 8 indirectly)
    public void GivenValidContext_WhenMove_ThenUpdatesPositionAsExpected()
    {
        var initialPosition = PositionalVector2.Zero;
        var direction = DirectionalVector2.UnitX;
        var speed = 2f;
        var movable = new BasicMovable(initialPosition, direction, speed);

        var contextMock = new Mock<IContext>().Object;

        movable.Move(contextMock);

        movable.Position.Should().Be(initialPosition + direction * speed);
    }

    [Fact] // 11 & 12
    public void GivenMultipleMoves_WhenMove_ThenPositionAccumulatesCorrectly_AndContextIsNotMutated()
    {
        var initialPosition = new PositionalVector2(1, 1);
        var direction = DirectionalVector2.UnitY;
        var speed = 3f;
        var movable = new BasicMovable(initialPosition, direction, speed);

        var contextMock = new Mock<IContext>();
        var originalShooterPosition = new PositionalVector2(99, 99);
        contextMock.SetupProperty(c => c.ShooterPosition, originalShooterPosition);

        movable.Move(contextMock.Object);
        movable.Move(contextMock.Object);

        movable.Position.Should().Be(initialPosition + direction * speed * 2);
        contextMock.Object.ShooterPosition.Should().Be(originalShooterPosition);
    }


    [Fact] // 13
    public void GivenZeroDirection_WhenMove_ThenPositionDoesNotChange()
    {
        var initialPosition = new PositionalVector2(5, 5);
        var direction = DirectionalVector2.Zero;
        var speed = 2f;
        var movable = new BasicMovable(initialPosition, direction, speed);

        var contextMock = new Mock<IContext>().Object;

        movable.Move(contextMock);

        movable.Position.Should().Be(initialPosition);
    }

    [Theory]
    [InlineData(100000, 100000, 1, 0, 2)]
    [InlineData(-1000, -1000, 0, 1, 5)]
    public void GivenEdgeCasePositions_WhenMove_ThenPositionUpdatesCorrectly(float px, float py, float dx, float dy, float speed)
    {
        var initialPosition = new PositionalVector2(px, py);
        var direction = new DirectionalVector2(dx, dy);
        var movable = new BasicMovable(initialPosition, direction, speed);

        var contextMock = new Mock<IContext>().Object;

        movable.Move(contextMock);

        movable.Position.Should().Be(initialPosition + direction * speed);
    }
    #endregion
}