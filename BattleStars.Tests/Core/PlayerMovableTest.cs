using System.Numerics;
using BattleStars.Core;
using BattleStars.Logic;
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
        // Arrange
        var invalidPositionVector = new Vector2(invalidPosition, 0);
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(invalidPositionVector, 1, mockBoundaryChecker.Object);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GivenInvalidSpeedInput_WhenConstructed_ThenThrowsOutOfRangeException(float invalidSpeed)
    {
        // Arrange
        var validPosition = new Vector2(0, 0);
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(validPosition, invalidSpeed, mockBoundaryChecker.Object);

        // Act/Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidSpeedInput_WhenConstructed_ThenThrowsException(float invalidSpeed)
    {
        // Arrange
        var validPosition = new Vector2(0, 0);
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(validPosition, invalidSpeed, mockBoundaryChecker.Object);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNullBoundaryChecker_WhenConstructed_ThenThrowsException()
    {
        // Arrange
        var validPosition = new Vector2(0, 0);
        var validSpeed = 1;
        Action act = () => new PlayerMovable(validPosition, validSpeed, null!);

        // Act/Assert
        act.Should().Throw<ArgumentNullException>();
    }


    [Fact]
    public void GivenValidInput_WhenConstructed_ThenSucceeds()
    {
        //Arrange
        var validPosition = new Vector2(0, 0);
        var validSpeed = 1;
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();

        Action act = () => new PlayerMovable(validPosition, validSpeed, mockBoundaryChecker.Object);

        //Act/Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Move Tests
    /*  Tests that the Move method behaves as expected.
        - Guards against null context.
        - Guards against NaN and Inf for player direction.
        - Guards against non-normalized/non-zero player direction.
        - Moves the player in the correct direction with the correct speed for valid input.
        - Clamps the player position when attempting to cross X boundary
        - Clamps the player position when attempting to cross Y boundary
        - Clamps the player position when attempting to cross both boundaries
    */

    [Fact]
    public void GivenNullContext_WhenMove_ThenThrowsException()
    {
        // Arrange
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, mockBoundaryChecker.Object);
        Action act = () => playerMovable.Move(null!);

        // Act/Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidDirection_WhenMove_ThenThrowsException(float invalidDirection)
    {
        // Arrange
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, mockBoundaryChecker.Object);
        var invalidDirectionVector = new Vector2(invalidDirection, 0);
        var context = new TestContextFixture(invalidDirectionVector);

        Action act = () => playerMovable.Move(context);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNonNormalizedDirection_WhenMove_ThenThrowsException()
    {
        // Arrange
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, mockBoundaryChecker.Object);
        var nonNormalizedDirection = new Vector2(1, 1);
        var context = new TestContextFixture(nonNormalizedDirection);

        Action act = () => playerMovable.Move(context);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidInput_WhenMove_ThenSucceeds()
    {
        //Arrange
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, mockBoundaryChecker.Object);
        var validDirection = new Vector2(1, 0);
        var context = new TestContextFixture(validDirection);

        Action act = () => playerMovable.Move(context);

        //Act/Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(0, 0, -1, 0, -1, 0)]     // Move Left
    [InlineData(0, 0, 1, 0, 1, 0)]     // Move Right
    [InlineData(0, 0, 0, 1, 0, 1)]     // Move Up
    [InlineData(0, 0, 0, -1, 0, -1)]     // Move Down
    [InlineData(0, 0, 1, 1, 0.7, 0.7)]   // Move Up and Right
    [InlineData(0, 0, -1, -1, -0.7, -0.7)]   // Move Down and Left
    [InlineData(0, 0, -1, 1, -0.7, 0.7)]   // Move Up and Left
    [InlineData(0, 0, 1, -1, 0.7, -0.7)]   // Move Down and Right
    [InlineData(0, 0, 0, 0, 0, 0)]   // Move Zero
    [InlineData(5, 5, 0, 1, 5, 6)]     // Move Up from non-origin
    [InlineData(5, 5, 0, -1, 5, 4)]     // Move Down from non-origin
    [InlineData(5, 5, 1, 0, 6, 5)]     // Move Right from non-origin
    [InlineData(5, 5, -1, 0, 4, 5)]     // Move Left from non-origin
    [InlineData(5, 5, 1, 1, 5.7, 5.7)]   // Move Up and Right from non-origin
    [InlineData(5, 5, -1, -1, 4.3, 4.3)]   // Move Down and Left from non-origin
    [InlineData(5, 5, -1, 1, 4.3, 5.7)]   // Move Up and Left from non-origin
    [InlineData(5, 5, 1, -1, 5.7, 4.3)]   // Move Down and Right from non-origin
    [InlineData(5, 5, 0, 0, 5, 5)]   // Move Zero from non-origin
    public void GivenValidInput_WhenMove_ThenPositionIsUpdated(float PX, float PY, float DX, float DY, float expectedX, float expectedY)
    {
        //Arrange
        var mockBoundaryChecker = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(PX, PY), 1, mockBoundaryChecker.Object);

        var direction = new Vector2(DX, DY);
        if (DX != 0 && DY != 0)
        {
            direction = Vector2.Normalize(direction);
        }
        var context = new TestContextFixture(direction);


        Action act = () => playerMovable.Move(context);

        //Act/Assert
        act.Should().NotThrow();
        playerMovable.Position.X.Should().BeApproximately(expectedX, 0.01f);
        playerMovable.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Fact]
    public void GivenPlayerMovable_WhenMovedMultipleTimes_PositionAccumulatesCorrectly()
    {
        // Arrange
        var noBoundaryEnforced = new Mock<IBoundaryChecker>();
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, noBoundaryEnforced.Object);
        var contextRight = new TestContextFixture(Vector2.UnitX);
        var contextUp = new TestContextFixture(Vector2.UnitY);

        // Act
        playerMovable.Move(contextRight);
        playerMovable.Move(contextUp);

        // Assert
        playerMovable.Position.Should().Be(new Vector2(1, 1));
    }

    [Theory]
    [InlineData(-1, 0, 0, 0)] // Move left across boundary
    [InlineData(1, 0, 1, 0)]   // Move right away from boundary
    [InlineData(-1, 1, 0, 0.7)] // Move left and down across boundary
    [InlineData(-1, -1, 0, -0.7)]   // Move left and up across boundary
    public void GivenPlayerMovable_WhenCrossingLeftBoundary_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var leftOnlyBoundaryChecker = new Mock<IBoundaryChecker>();
        leftOnlyBoundaryChecker.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns<float>(x => x < 0);
        leftOnlyBoundaryChecker.Setup(b => b.XDistanceToBoundary(It.IsAny<float>())).Returns<float>(x => -x);
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, leftOnlyBoundaryChecker.Object);
        var context = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        playerMovable.Move(context);

        // Assert
        playerMovable.Position.X.Should().BeApproximately(expectedX, 0.01f);
        playerMovable.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Theory]
    [InlineData(-1, -1, 0, 0)] // Move diagonally left-up across both boundaries
    [InlineData(-1, 1, 0, 0.7)] // Move diagonally left-down across X boundary only
    [InlineData(1, -1, 0.7, 0)] // Move diagonally right-up across Y boundary only
    public void GivenPlayerMovable_WhenCrossingMultipleBoundaries_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var boundaryChecker = new Mock<IBoundaryChecker>();
        boundaryChecker.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns<float>(x => x < 0);
        boundaryChecker.Setup(b => b.XDistanceToBoundary(It.IsAny<float>())).Returns<float>(x => -x);
        boundaryChecker.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns<float>(y => y < 0);
        boundaryChecker.Setup(b => b.YDistanceToBoundary(It.IsAny<float>())).Returns<float>(y => -y);

        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, boundaryChecker.Object);
        var context = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        playerMovable.Move(context);

        // Assert
        playerMovable.Position.X.Should().BeApproximately(expectedX, 0.01f);
        playerMovable.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Theory]
    [InlineData(0, -1, 0, 0)] // Move up across boundary
    [InlineData(0, 1, 0, 1)]   // Move down away from boundary
    [InlineData(-1, -1, -0.7, 0)] // Move left and up across boundary
    [InlineData(1, -1, 0.7, 0)]   // Move right and up across boundary
    public void GivenPlayerMovable_WhenCrossingYBoundary_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var leftOnlyBoundaryChecker = new Mock<IBoundaryChecker>();
        leftOnlyBoundaryChecker.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns<float>(y => y < 0);
        leftOnlyBoundaryChecker.Setup(b => b.YDistanceToBoundary(It.IsAny<float>())).Returns<float>(y => -y);
        var playerMovable = new PlayerMovable(new Vector2(0, 0), 1, leftOnlyBoundaryChecker.Object);
        var context = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        playerMovable.Move(context);

        // Assert
        playerMovable.Position.X.Should().BeApproximately(expectedX, 0.01f);
        playerMovable.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }
    
    #endregion
}