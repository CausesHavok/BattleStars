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

        public Vector2 ShooterPosition { get; set; }

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(invalidPositionVector, 1, defaultBoundaryCheckerMock.Object);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(validPosition, invalidSpeed, defaultBoundaryCheckerMock.Object);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        Action act = () => new PlayerMovable(validPosition, invalidSpeed, defaultBoundaryCheckerMock.Object);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();

        Action act = () => new PlayerMovable(validPosition, validSpeed, defaultBoundaryCheckerMock.Object);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(0, 0), 1, defaultBoundaryCheckerMock.Object);
        Action act = () => sut.Move(null!);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(0, 0), 1, defaultBoundaryCheckerMock.Object);
        var invalidDirectionVector = new Vector2(invalidDirection, 0);
        var invalidDirectionContext = new TestContextFixture(invalidDirectionVector);

        Action act = () => sut.Move(invalidDirectionContext);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNonNormalizedDirection_WhenMove_ThenThrowsException()
    {
        // Arrange
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(0, 0), 1, defaultBoundaryCheckerMock.Object);
        var nonNormalizedDirection = new Vector2(1, 1);
        var nonNormalizedDirectionContext = new TestContextFixture(nonNormalizedDirection);

        Action act = () => sut.Move(nonNormalizedDirectionContext);

        // Act/Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidInput_WhenMove_ThenSucceeds()
    {
        //Arrange
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(0, 0), 1, defaultBoundaryCheckerMock.Object);
        var validDirection = new Vector2(1, 0);
        var validDirectionContext = new TestContextFixture(validDirection);

        Action act = () => sut.Move(validDirectionContext);

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
        var defaultBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(PX, PY), 1, defaultBoundaryCheckerMock.Object);

        var direction = new Vector2(DX, DY);
        if (DX != 0 && DY != 0)
        {
            direction = Vector2.Normalize(direction);
        }
        var inputDirectedContext = new TestContextFixture(direction);


        Action act = () => sut.Move(inputDirectedContext);

        //Act/Assert
        act.Should().NotThrow();
        sut.Position.X.Should().BeApproximately(expectedX, 0.01f);
        sut.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Fact]
    public void GivenPlayerMovable_WhenMovedMultipleTimes_PositionAccumulatesCorrectly()
    {
        // Arrange
        var noBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        var sut = new PlayerMovable(new Vector2(0, 0), 1, noBoundaryCheckerMock.Object);
        var rightDirectionContext = new TestContextFixture(Vector2.UnitX);
        var upDirectionContext = new TestContextFixture(Vector2.UnitY);

        // Act
        sut.Move(rightDirectionContext);
        sut.Move(upDirectionContext);

        // Assert
        sut.Position.Should().Be(new Vector2(1, 1));
    }

    [Theory]
    [InlineData(-1, 0, 0, 0)] // Move left across boundary
    [InlineData(1, 0, 1, 0)]   // Move right away from boundary
    [InlineData(-1, 1, 0, 0.7)] // Move left and down across boundary
    [InlineData(-1, -1, 0, -0.7)]   // Move left and up across boundary
    public void GivenPlayerMovable_WhenCrossingLeftBoundary_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var leftBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        leftBoundaryCheckerMock.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns<float>(x => x < 0);
        leftBoundaryCheckerMock.Setup(b => b.XDistanceToBoundary(It.IsAny<float>())).Returns<float>(x => -x);
        var sut = new PlayerMovable(new Vector2(0, 0), 1, leftBoundaryCheckerMock.Object);
        var inputDirectedContext = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        sut.Move(inputDirectedContext);

        // Assert
        sut.Position.X.Should().BeApproximately(expectedX, 0.01f);
        sut.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Theory]
    [InlineData(-1, -1, 0, 0)] // Move diagonally left-up across both boundaries
    [InlineData(-1, 1, 0, 0.7)] // Move diagonally left-down across X boundary only
    [InlineData(1, -1, 0.7, 0)] // Move diagonally right-up across Y boundary only
    public void GivenPlayerMovable_WhenCrossingMultipleBoundaries_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var leftAndUpBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        leftAndUpBoundaryCheckerMock.Setup(b => b.IsOutsideXBounds(It.IsAny<float>())).Returns<float>(x => x < 0);
        leftAndUpBoundaryCheckerMock.Setup(b => b.XDistanceToBoundary(It.IsAny<float>())).Returns<float>(x => -x);
        leftAndUpBoundaryCheckerMock.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns<float>(y => y < 0);
        leftAndUpBoundaryCheckerMock.Setup(b => b.YDistanceToBoundary(It.IsAny<float>())).Returns<float>(y => -y);

        var sut = new PlayerMovable(new Vector2(0, 0), 1, leftAndUpBoundaryCheckerMock.Object);
        var inputDirectedContext = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        sut.Move(inputDirectedContext);

        // Assert
        sut.Position.X.Should().BeApproximately(expectedX, 0.01f);
        sut.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }

    [Theory]
    [InlineData(0, -1, 0, 0)] // Move up across boundary
    [InlineData(0, 1, 0, 1)]   // Move down away from boundary
    [InlineData(-1, -1, -0.7, 0)] // Move left and up across boundary
    [InlineData(1, -1, 0.7, 0)]   // Move right and up across boundary
    public void GivenPlayerMovable_WhenCrossingYBoundary_ThenPositionIsProjected(float directionX, float directionY, float expectedX, float expectedY)
    {
        // Arrange
        var upBoundaryCheckerMock = new Mock<IBoundaryChecker>();
        upBoundaryCheckerMock.Setup(b => b.IsOutsideYBounds(It.IsAny<float>())).Returns<float>(y => y < 0);
        upBoundaryCheckerMock.Setup(b => b.YDistanceToBoundary(It.IsAny<float>())).Returns<float>(y => -y);
        var sut = new PlayerMovable(new Vector2(0, 0), 1, upBoundaryCheckerMock.Object);
        var inputDirectedContext = new TestContextFixture(Vector2.Normalize(new Vector2(directionX, directionY)));

        // Act
        sut.Move(inputDirectedContext);

        // Assert
        sut.Position.X.Should().BeApproximately(expectedX, 0.01f);
        sut.Position.Y.Should().BeApproximately(expectedY, 0.01f);
    }
    
    #endregion
}