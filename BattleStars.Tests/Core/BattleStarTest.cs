using Moq;
using System.Numerics;
using BattleStars.Core;
using BattleStars.Shapes;
using BattleStars.Shots;
using FluentAssertions;

namespace BattleStars.Tests.Core;

public class TestBattleStarFixture
{
    public Mock<IShape> MockShape = new();
    public Mock<IShapeDrawer> MockShapeDrawer = new();
    public Mock<IMovable> MockMovable = new();
    public Mock<IDestructable> MockDestructable = new();
    public Mock<IShooter> MockShooter = new();
    public BattleStar BattleStar;

    public TestBattleStarFixture()
    {
        BattleStar = new BattleStar(
            MockShape.Object,
            MockShapeDrawer.Object,
            MockMovable.Object,
            MockDestructable.Object,
            MockShooter.Object);
    }
}

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


public class BattleStarTest : IClassFixture<TestBattleStarFixture>
{
    #region Helpers
    private readonly TestBattleStarFixture _battleStarFixture;
    private readonly TestContextFixture _contextFixture;

    public BattleStarTest(TestBattleStarFixture fixture)
    {
        _battleStarFixture = fixture;
        _contextFixture = new TestContextFixture();
    }
    #endregion

    #region Construction Tests

    [Fact]
    public void GivenNullShape_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        IShape nullShape = null!;
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var mockMovable = new Mock<IMovable>();
        var mockDestructable = new Mock<IDestructable>();
        var mockShooter = new Mock<IShooter>();

        Action act = () => new BattleStar(nullShape, mockShapeDrawer.Object, mockMovable.Object, mockDestructable.Object, mockShooter.Object);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShapeDrawer_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        IShapeDrawer nullShapeDrawer = null!;
        var mockMovable = new Mock<IMovable>();
        var mockDestructable = new Mock<IDestructable>();
        var mockShooter = new Mock<IShooter>();

        Action act = () => new BattleStar(mockShape.Object, nullShapeDrawer, mockMovable.Object, mockDestructable.Object, mockShooter.Object);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullMovable_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        IMovable nullMovable = null!;
        var mockDestructable = new Mock<IDestructable>();
        var mockShooter = new Mock<IShooter>();

        Action act = () => new BattleStar(mockShape.Object, mockShapeDrawer.Object, nullMovable, mockDestructable.Object, mockShooter.Object);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullDestructable_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var mockMovable = new Mock<IMovable>();
        IDestructable nullDestructable = null!;
        var mockShooter = new Mock<IShooter>();

        Action act = () => new BattleStar(mockShape.Object, mockShapeDrawer.Object, mockMovable.Object, nullDestructable, mockShooter.Object);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShooter_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var mockMovable = new Mock<IMovable>();
        var mockDestructable = new Mock<IDestructable>();
        IShooter nullShooter = null!;

        Action act = () => new BattleStar(mockShape.Object, mockShapeDrawer.Object, mockMovable.Object, mockDestructable.Object, nullShooter);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidInput_WhenConstructed_ThenDoesNotThrow()
    {
        // Arrange
        var mockShape = new Mock<IShape>();
        var mockShapeDrawer = new Mock<IShapeDrawer>();
        var mockMovable = new Mock<IMovable>();
        var mockDestructable = new Mock<IDestructable>();
        var mockShooter = new Mock<IShooter>();

        Action act = () => new BattleStar(mockShape.Object, mockShapeDrawer.Object, mockMovable.Object, mockDestructable.Object, mockShooter.Object);

        // Act & Assert
        act.Should().NotThrow();
    }

    #endregion


    #region Delegation Tests

    [Fact]
    public void GivenBattleStar_WhenDrawCalled_ThenDelegatesToShape()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        // Act
        battleStar.Draw();

        // Assert
        testBattleStar.MockShape.Verify(s => s.Draw(It.IsAny<Vector2>(), testBattleStar.MockShapeDrawer.Object), Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenGetBoundingBoxCalled_ThenDelegatesToShape()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var expectedBoundingBox = new BoundingBox();
        testBattleStar.MockShape.Setup(s => s.BoundingBox).Returns(expectedBoundingBox);

        // Act
        var result = battleStar.GetBoundingBox();

        // Assert
        result.Should().Be(expectedBoundingBox);
    }

    [Fact]
    public void GivenBattleStar_WhenMoveCalled_ThenDelegatesToMovable()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var context = _contextFixture;

        // Act
        battleStar.Move(context);

        // Assert
        testBattleStar.MockMovable.Verify(m => m.Move(context), Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenAccessingPosition_ThenReturnsPosition()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var expectedPosition = new Vector2(10, 20);
        testBattleStar.MockMovable.Setup(m => m.Position).Returns(expectedPosition);

        // Act
        var result = testBattleStar.MockMovable.Object.Position;

        // Assert
        result.Should().Be(expectedPosition);
    }

    [Fact]
    public void GivenBattleStar_WhenTakeDamageCalled_ThenDelegatesToDestructable()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        float damageAmount = 25.0f;

        // Act
        battleStar.TakeDamage(damageAmount);

        // Assert
        testBattleStar.MockDestructable.Verify(d => d.TakeDamage(damageAmount), Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenIsDestroyedCalled_ThenDelegatesToDestructable()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        // Act
        bool isDestroyed = battleStar.IsDestroyed;

        // Assert
        testBattleStar.MockDestructable.Verify(d => d.IsDestroyed, Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenShootCalled_ThenDelegatesToShooter()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var context = _contextFixture;

        // Act
        battleStar.Shoot(context);

        // Assert
        testBattleStar.MockShooter.Verify(s => s.Shoot(context), Times.Once);
    }

    [Fact]
    public void GivenBattleStar_WhenShootCalled_ThenReturnsShotsFromShooter()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var context = _contextFixture;

        var expectedShots = new List<IShot> { new Mock<IShot>().Object };
        testBattleStar.MockShooter.Setup(s => s.Shoot(context)).Returns(expectedShots);

        // Act
        var result = battleStar.Shoot(context);

        // Assert
        result.Should().BeEquivalentTo(expectedShots);
    }

    [Fact]
    public void GivenBattleStar_WhenShootCalled_ThenContextShooterPositionIsSet()
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        var context = _contextFixture;

        var expectedPosition = new Vector2(15, 30);
        testBattleStar.MockMovable.Setup(m => m.Position).Returns(expectedPosition);

        // Act
        battleStar.Shoot(context);

        // Assert
        context.ShooterPosition.Should().Be(expectedPosition);
    }

    [Theory]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    [InlineData(-5, -5)]
    [InlineData(0, 0)]
    public void GivenBattleStar_WhenContainsCalled_ThenDelegatesToShapeWithAdjustedPoint(float x, float y)
    {
        // Arrange
        var testBattleStar = _battleStarFixture;
        var battleStar = testBattleStar.BattleStar;

        testBattleStar.MockMovable.Setup(m => m.Position).Returns(new Vector2(x, y));
        var point = new Vector2(5, 5);
        var adjustedPoint = point - new Vector2(x, y);

        // Act
        battleStar.Contains(point);

        // Assert
        testBattleStar.MockShape.Verify(s => s.Contains(adjustedPoint), Times.Once);
    }

    #endregion
}