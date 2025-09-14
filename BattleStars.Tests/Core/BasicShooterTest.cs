using System.Numerics;
using FluentAssertions;
using Moq;
using BattleStars.Core;
using BattleStars.Shots;

namespace BattleStars.Tests.Core;

public class BasicShooterTest
{
    #region Constructor Tests
    /*
        1. Given null shotFactory, when constructed, then throws ArgumentNullException.
        2. Given direction is NaN, when constructed, then throws ArgumentException.
        3. Given direction is Infinity, when constructed, then throws ArgumentException.
        4. Given non-zero non-normalized direction, when constructed, then throws ArgumentException.
        5. Given valid normalized direction, when constructed, then does not throw.
        6. Given zero direction, when constructed, then does not throw.
    */

    [Fact] // 1
    public void GivenNullShotFactory_WhenConstructed_ThenThrowsArgumentNullException()
    {
        Func<Vector2, Vector2, IShot> shotFactory = null!;
        var direction = Vector2.UnitY;
        Action act = () => new BasicShooter(shotFactory, direction);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory] // 2, 3
    [InlineData(float.NaN, 0)]
    [InlineData(0, float.NaN)]
    [InlineData(float.PositiveInfinity, 0)]
    [InlineData(0, float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity, 0)]
    [InlineData(0, float.NegativeInfinity)]
    public void GivenDirectionIsNaNOrInfinity_WhenConstructed_ThenThrowsArgumentException(float x, float y)
    {
        var shotFactory = new Mock<Func<Vector2, Vector2, IShot>>().Object;
        var direction = new Vector2(x, y);
        Action act = () => new BasicShooter(shotFactory, direction);
        act.Should().Throw<ArgumentException>();
    }

    [Theory] // 4
    [InlineData(2, 0)]
    [InlineData(0, -2)]
    public void GivenNonZeroNonNormalizedDirection_WhenConstructed_ThenThrowsArgumentException(float x, float y)
    {
        var shotFactory = new Mock<Func<Vector2, Vector2, IShot>>().Object;
        var direction = new Vector2(x, y);
        Action act = () => new BasicShooter(shotFactory, direction);
        act.Should().Throw<ArgumentException>();
    }

    [Theory] // 5 and 6
    [InlineData(1, 0)]
    [InlineData(0, -1)]
    [InlineData(0.7071f, 0.7071f)] // Approx sqrt(2)/2
    [InlineData(0, 0)] // Added to cover zero direction case
    public void GivenValidDirection_WhenConstructed_ThenDoesNotThrow(float x, float y)
    {
        var shotFactory = new Mock<Func<Vector2, Vector2, IShot>>().Object;
        var direction = new Vector2(x, y);
        Action act = () => new BasicShooter(shotFactory, direction);
        act.Should().NotThrow();
    }

    #endregion

    #region Shoot Method Tests
    /*
        07. Given valid inputs, when constructed, then stores parameters correctly.
        08. Given null context, when Shoot is called, then throws ArgumentNullException.
        09. Given valid context, when Shoot is called, then shotFactory is called once with correct parameters.
        10. Given valid context, when Shoot is called, then returns a single non-null shot.
        11. Given valid context, when Shoot is called, then the returned shot is the one produced by the shotFactory.
        12. Given context with specific ShooterPosition, when Shoot is called, then shot position equals context ShooterPosition.
        13. Given specific direction in constructor, when Shoot is called, then shot direction equals constructor direction.
        14. Given multiple calls to Shoot, when called multiple times, then produces new shots each time.
        15. Given context position changes between calls, when Shoot is called, then shot position reflects new value.
        16. Given shotFactory throws exception, when Shoot is called, then exception is propagated.
        17. Given context, when Shoot is called, then context is not mutated.
        18. Given direction, when Shoot is called, then direction is not mutated.
        19. Given edge case positions (e.g., very large or small values), when Shoot is called, then shot position is correct.
        20. Given zero direction in constructor, when Shoot is called, then shot direction is zero.
        21. Given valid context and multiple calls to Shoot, when Shoot is called multiple times, then shotFactory is called with correct parameters each time.
        22. Given custom implementation of IContext, when Shoot is called, then works correctly with custom context.
    */

    [Theory] // 7, 9, 12, 13, and 21
    [InlineData(1, 2, 0, 1)]      // position (1,2), direction (0,1)
    [InlineData(42, 99, 1, 0)]    // position (42,99), direction (1,0)
    [InlineData(-5, -5, 0, -1)]   // position (-5,-5), direction (0,-1)
    public void GivenValidInputAndContext_WhenShootCalledMultipleTimes_ThenShotFactoryIsCalledWithStoredDirectionAndContextPosition(
        float px, float py, float dx, float dy)
    {
        // Arrange
        var expectedPosition = new Vector2(px, py);
        var expectedDirection = new Vector2(dx, dy);
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(expectedPosition);

        // Act
        var result1 = shooter.Shoot(contextMock.Object);
        var result2 = shooter.Shoot(contextMock.Object);

        // Assert
        result1.Should().ContainSingle().Which.Should().Be(expectedShot);
        result2.Should().ContainSingle().Which.Should().Be(expectedShot);
        shotFactoryMock.Verify(f => f(expectedPosition, expectedDirection), Times.Exactly(2));
    }

    [Fact] // 8
    public void GivenNullContext_WhenShoot_ThenThrowsArgumentNullException()
    {
        var shotFactory = new Mock<Func<Vector2, Vector2, IShot>>().Object;
        var direction = Vector2.UnitY;
        var shooter = new BasicShooter(shotFactory, direction);
        Action act = () => shooter.Shoot(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact] // 10, 11, 20
    public void GivenValidContext_WhenShoot_ThenReturnsSingleNonNullShot()
    {
        // Arrange
        var expectedPosition = new Vector2(5, 5);
        var expectedDirection = Vector2.Zero;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(expectedPosition);

        // Act
        var result = shooter.Shoot(contextMock.Object);

        // Assert
        result.Should().ContainSingle().Which.Should().Be(expectedShot);
    }

    [Fact] // 14
    public void GivenMultipleCallsToShoot_WhenCalled_ThenProducesNewShotsEachTime()
    {
        // Arrange
        var expectedPosition = new Vector2(2, 2);
        var expectedDirection = Vector2.UnitY;
        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(new Mock<IShot>().Object);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(expectedPosition);

        // Act
        var result1 = shooter.Shoot(contextMock.Object);
        var result2 = shooter.Shoot(contextMock.Object);

        // Assert
        result1.Should().ContainSingle();
        result2.Should().ContainSingle();
        shotFactoryMock.Verify(f => f(expectedPosition, expectedDirection), Times.Exactly(2));
    }

    [Fact] // 15
    public void GivenContextPositionChangesBetweenCalls_WhenShoot_ThenShotPositionReflectsNewValue()
    {
        // Arrange
        var expectedDirection = Vector2.UnitY;
        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(It.IsAny<Vector2>(), expectedDirection)).Returns(new Mock<IShot>().Object);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.SetupSequence(c => c.ShooterPosition)
            .Returns(new Vector2(1, 1))
            .Returns(new Vector2(2, 2));

        // Act
        shooter.Shoot(contextMock.Object);
        shooter.Shoot(contextMock.Object);

        // Assert
        shotFactoryMock.Verify(f => f(new Vector2(1, 1), expectedDirection), Times.Once);
        shotFactoryMock.Verify(f => f(new Vector2(2, 2), expectedDirection), Times.Once);
    }

    [Fact] // 16
    public void GivenShotFactoryThrows_WhenShoot_ThenExceptionIsPropagated()
    {
        // Arrange
        var expectedDirection = Vector2.UnitY;
        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(It.IsAny<Vector2>(), expectedDirection)).Throws(new InvalidOperationException());

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(new Vector2(0, 0));

        Action act = () => shooter.Shoot(contextMock.Object);

        // Act & Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact] // 17
    public void GivenContext_WhenShoot_ThenContextIsNotMutated()
    {
        // Arrange
        var expectedPosition = new Vector2(1, 1);
        var expectedDirection = Vector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.SetupProperty(c => c.ShooterPosition, expectedPosition);

        // Act
        shooter.Shoot(contextMock.Object);

        // Assert
        contextMock.Object.ShooterPosition.Should().Be(expectedPosition);
    }

    [Fact] // 18
    public void GivenShootCalled_WhenShoot_ThenDirectionIsNotMutated()
    {
        // Arrange
        var expectedPosition = new Vector2(1, 1);
        var expectedDirection = Vector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(expectedPosition);

        shooter.Shoot(contextMock.Object);

        // Act
        var result = shooter.Shoot(contextMock.Object);

        // Assert
        shotFactoryMock.Verify(f => f(expectedPosition, expectedDirection), Times.Exactly(2));
    }

    [Theory] // 19
    [InlineData(-1000, -1000)]
    [InlineData(100000, 100000)]
    public void GivenEdgeCasePositions_WhenShoot_ThenShotPositionIsCorrect(float x, float y)
    {
        // Arrange
        var expectedPosition = new Vector2(x, y);
        var expectedDirection = Vector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(expectedPosition);

        // Act
        var result = shooter.Shoot(contextMock.Object);

        // Assert
        result.Should().ContainSingle().Which.Should().Be(expectedShot);
    }

    [Fact] // 22
    public void GivenCustomContextImplementation_WhenShoot_ThenWorksCorrectly()
    {
        // Arrange
        var expectedPosition = new Vector2(7, 8);
        var expectedDirection = Vector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<Vector2, Vector2, IShot>>();
        shotFactoryMock.Setup(f => f(expectedPosition, expectedDirection)).Returns(expectedShot);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var customContext = new CustomContext(expectedPosition);

        // Act
        var result = shooter.Shoot(customContext);

        // Assert
        result.Should().ContainSingle().Which.Should().Be(expectedShot);
    }

    private class CustomContext : IContext
    {
        public CustomContext(Vector2 shooterPosition)
        {
            ShooterPosition = shooterPosition;
        }
        public Vector2 ShooterPosition { get; set; }
        public Vector2 PlayerDirection => Vector2.UnitX; // Arbitrary implementation
    }

    #endregion
}