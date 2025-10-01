using FluentAssertions;
using Moq;
using BattleStars.Domain.Entities;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.Entities;

public class BasicShooterTest
{
    #region Constructor Tests
    /*
        1. Given null shotFactory, when constructed, then throws ArgumentNullException.
        5. Given valid normalized direction, when constructed, then does not throw.
        6. Given zero direction, when constructed, then does not throw.
    */

    [Fact] // 1
    public void GivenNullShotFactory_WhenConstructed_ThenThrowsArgumentNullException()
    {
        Func<PositionalVector2, DirectionalVector2, IShot> shotFactory = null!;
        var direction = DirectionalVector2.UnitY;
        Action act = () => new BasicShooter(shotFactory, direction);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory] // 5 and 6
    [InlineData(1, 0)]
    [InlineData(0, -1)]
    [InlineData(0.7071f, 0.7071f)] // Approx sqrt(2)/2
    [InlineData(0, 0)] // Added to cover zero direction case
    public void GivenValidDirection_WhenConstructed_ThenDoesNotThrow(float x, float y)
    {
        var shotFactory = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>().Object;
        var direction = new DirectionalVector2(x, y);
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
        var expectedPosition = new PositionalVector2(px, py);
        var expectedDirection = new DirectionalVector2(dx, dy);
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var shotFactory = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>().Object;
        var direction = DirectionalVector2.UnitY;
        var shooter = new BasicShooter(shotFactory, direction);
        Action act = () => shooter.Shoot(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact] // 10, 11, 20
    public void GivenValidContext_WhenShoot_ThenReturnsSingleNonNullShot()
    {
        // Arrange
        var expectedPosition = new PositionalVector2(5, 5);
        var expectedDirection = DirectionalVector2.Zero;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var expectedPosition = new PositionalVector2(2, 2);
        var expectedDirection = DirectionalVector2.UnitY;
        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var expectedDirection = DirectionalVector2.UnitY;
        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
        shotFactoryMock.Setup(f => f(It.IsAny<PositionalVector2>(), expectedDirection)).Returns(new Mock<IShot>().Object);

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.SetupSequence(c => c.ShooterPosition)
            .Returns(new PositionalVector2(1, 1))
            .Returns(new PositionalVector2(2, 2));

        // Act
        shooter.Shoot(contextMock.Object);
        shooter.Shoot(contextMock.Object);

        // Assert
        shotFactoryMock.Verify(f => f(new PositionalVector2(1, 1), expectedDirection), Times.Once);
        shotFactoryMock.Verify(f => f(new PositionalVector2(2, 2), expectedDirection), Times.Once);
    }

    [Fact] // 16
    public void GivenShotFactoryThrows_WhenShoot_ThenExceptionIsPropagated()
    {
        // Arrange
        var expectedDirection = DirectionalVector2.UnitY;
        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
        shotFactoryMock.Setup(f => f(It.IsAny<PositionalVector2>(), expectedDirection)).Throws(new InvalidOperationException());

        var shooter = new BasicShooter(shotFactoryMock.Object, expectedDirection);

        var contextMock = new Mock<IContext>();
        contextMock.Setup(c => c.ShooterPosition).Returns(PositionalVector2.Zero);

        Action act = () => shooter.Shoot(contextMock.Object);

        // Act & Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact] // 17
    public void GivenContext_WhenShoot_ThenContextIsNotMutated()
    {
        // Arrange
        var expectedPosition = new PositionalVector2(1, 1);
        var expectedDirection = DirectionalVector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var expectedPosition = new PositionalVector2(1, 1);
        var expectedDirection = DirectionalVector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var expectedPosition = new PositionalVector2(x, y);
        var expectedDirection = DirectionalVector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        var expectedPosition = new PositionalVector2(7, 8);
        var expectedDirection = DirectionalVector2.UnitY;
        var expectedShot = new Mock<IShot>().Object;

        var shotFactoryMock = new Mock<Func<PositionalVector2, DirectionalVector2, IShot>>();
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
        public CustomContext(PositionalVector2 shooterPosition)
        {
            ShooterPosition = shooterPosition;
        }
        public PositionalVector2 ShooterPosition { get; set; }
        public DirectionalVector2 PlayerDirection { get => DirectionalVector2.UnitX; set => throw new NotImplementedException(); }
    }

    #endregion
}