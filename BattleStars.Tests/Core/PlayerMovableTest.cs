using System.Numerics;
using BattleStars.Core;
using FluentAssertions;
using Moq;

namespace BattleStars.Tests.Core;

public class PlayerMovableTest
{

    #region Helpers

    public class TestContextFixture : IContext
    {
        public Vector2 PlayerDirection { get; private set; }

        public TestContextFixture(Vector2 playerDirection)
        {
            PlayerDirection = playerDirection;
        }

        public TestContextFixture() : this(new Vector2(1, 0)) { }
    }

    #endregion


    #region Construction Tests
    /*  Tests that construction of the PlayerMovable class behaves as expected.
        - Guards against NaN and Inf for player position.
        - Guards against NaN, Inf, Zero and Negative values for player speed.
        - Sets Speed and Position for valid input.
    */

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidPositionInput_WhenConstructed_ThenThrowsException(float invalidPosition)
    {
        //Arrange
        var invalidPositionVector = new Vector2(invalidPosition, 0);
        Action act = () => new PlayerMovable(invalidPositionVector, 1);

        //Act/Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenInvalidSpeedInput_WhenConstructed_ThenThrowsOutOfRangeException(float invalidSpeed)
    {
        //Arrange
        var validPosition = new Vector2(0, 0);
        Action act = () => new PlayerMovable(validPosition, invalidSpeed);

        //Act/Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]

    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidSpeedInput_WhenConstructed_ThenThrowsException(float invalidSpeed)
    {
        //Arrange
        var validPosition = new Vector2(0, 0);
        Action act = () => new PlayerMovable(validPosition, invalidSpeed);

        //Act/Assert
        Assert.Throws<ArgumentException>(act);
    }


    [Fact]
    public void GivenValidInput_WhenConstructed_ThenSucceeds()
    {
        //Arrange
        var validPosition = new Vector2(0, 0);
        var validSpeed = 1;

        Action act = () => new PlayerMovable(validPosition, validSpeed);

        //Act/Assert
        var playerMovable = act.Should().NotThrow();
    }

    #endregion

    #region Move Tests
    /*  Tests that the Move method behaves as expected.
        - Guards against null context.
        - Guards against NaN and Inf for player direction.
        - Guards against non-normalized player direction.
        - Moves the player in the correct direction with the correct speed for valid input.
    */

    [Fact]
    public void GivenNullContext_WhenMove_ThenThrowsException()
    {
        //Arrange
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1);
        Action act = () => playerMovable.Move(null!);

        //Act/Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidDirection_WhenMove_ThenThrowsException(float invalidDirection)
    {
        //Arrange
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1);
        var invalidDirectionVector = new Vector2(invalidDirection, 0);
        var context = new TestContextFixture(invalidDirectionVector);

        Action act = () => playerMovable.Move(context);

        //Act/Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void GivenNonNormalizedDirection_WhenMove_ThenThrowsException()
    {
        //Arrange
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1);
        var nonNormalizedDirection = new Vector2(1, 1);
        var context = new TestContextFixture(nonNormalizedDirection);

        Action act = () => playerMovable.Move(context);

        //Act/Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void GivenValidInput_WhenMove_ThenSucceeds()
    {
        //Arrange
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1);
        var validDirection = new Vector2(1, 0);
        var context = new TestContextFixture(validDirection);

        Action act = () => playerMovable.Move(context);

        //Act/Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(0, 0, -1,  0,   -1,  0)]     // Move Left
    [InlineData(0, 0,  1,  0,    1,  0)]     // Move Right
    [InlineData(0, 0,  0,  1,    0,  1)]     // Move Up
    [InlineData(0, 0,  0, -1,    0, -1)]     // Move Down
    [InlineData(0, 0,  1,  1,  0.7,  0.7)]   // Move Up and Right
    [InlineData(0, 0, -1, -1, -0.7, -0.7)]   // Move Down and Left
    [InlineData(0, 0, -1,  1, -0.7,  0.7)]   // Move Up and Left
    [InlineData(0, 0,  1, -1,  0.7, -0.7)]   // Move Down and Right
    [InlineData(5, 5,  0,  1,    5,  6)]     // Move Up from non-origin
    [InlineData(5, 5,  0, -1,    5,  4)]     // Move Down from non-origin
    [InlineData(5, 5,  1,  0,    6,  5)]     // Move Right from non-origin
    [InlineData(5, 5, -1,  0,    4,  5)]     // Move Left from non-origin
    [InlineData(5, 5,  1,  1,  5.7,  5.7)]   // Move Up and Right from non-origin
    [InlineData(5, 5, -1, -1,  4.3,  4.3)]   // Move Down and Left from non-origin
    [InlineData(5, 5, -1,  1,  4.3,  5.7)]   // Move Up and Left from non-origin
    [InlineData(5, 5,  1, -1,  5.7,  4.3)]   // Move Down and Right from non-origin
    public void GivenValidInput_WhenMove_ThenPositionIsUpdated(float PX, float PY, float SX, float SY, float expectedX, float expectedY)
    {
        //Arrange
        var playerMovable = new PlayerMovable(new Vector2(PX, PY), 1);
        var validDirection = Vector2.Normalize(new Vector2(SX, SY));
        var context = new TestContextFixture(validDirection);

        Action act = () => playerMovable.Move(context);

        //Act/Assert
        act.Should().NotThrow();
        playerMovable.Position.X.Should().BeApproximately(expectedX, 0.01f);
        playerMovable.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Fact]
    public void GivenPlayerMovable_WhenMovedMultipleTimes_PositionAccumulatesCorrectly()
    {
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1);
        var contextRight = new TestContextFixture(Vector2.UnitX);
        var contextUp = new TestContextFixture(Vector2.UnitY);

        playerMovable.Move(contextRight);
        playerMovable.Move(contextUp);

        playerMovable.Position.Should().Be(new Vector2(1, 1));
    }

    #endregion
}