using System.Drawing;
using FluentAssertions;
using BattleStars.Domain.Entities.Shapes;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Tests.Entities.Shapes;

public class CircleTest
{

    public class MockShapeDrawer : IShapeDrawer
    {
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color)
        {
            DrawCalled = true;
        }

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color)
        {
            DrawCalled = true;
        }

        public void DrawCircle(PositionalVector2 center, float radius, Color color)
        {
            DrawCalled = true;
        }
    }

    #region Constructor Tests
    /*
        - Test the Circle constructor with valid parameters
        - Test the Circle constructor with invalid parameters
        - Test the Circle constructor with NaN, Infinity, negative, and zero radius
        - Test the Circle constructor with null IShapeDrawer
    */

    [Fact]
    public void GivenCircle_WhenConstructor_WithInvalidRadius_ThenThrowsArgumentException()
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        Action act = () => new Circle(float.NaN, Color.Red, mockShapeDrawer);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("radius cannot be NaN.*")
            .And.ParamName.Should().Be("radius");

        // Arrange
        act = () => new Circle(float.PositiveInfinity, Color.Red, mockShapeDrawer);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("radius must be finite.*")
            .And.ParamName.Should().Be("radius");

        // Arrange
        act = () => new Circle(float.NegativeInfinity, Color.Red, mockShapeDrawer);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("radius must be finite.*")
            .And.ParamName.Should().Be("radius");
    }

    [Theory]
    [InlineData(0.0f, "zero")]
    [InlineData(-1.0f, "negative")]
    public void GivenCircle_WhenConstructed_WithZeroOrNegativeRadius_ThenThrowsArgumentOutOfRangeException(float radius, string expectedException)
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        Action act = () => new Circle(radius, Color.Red, mockShapeDrawer);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("radius cannot be " + expectedException + ".*")
            .And.ParamName.Should().Be("radius");
    }

    [Fact]
    public void GivenCircle_WhenConstructedWithNullShapeDrawer_ThenThrowsArgumentNullException()
    {
        Action act = () => new Circle(5, Color.Red, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null.*")
            .And.ParamName.Should().Be("drawer");
    }

    #endregion

    #region BoundingBox Tests
    /*
        - Test that bounding box is correctly calculated for a circle with positive radius
        - Test that bounding box is correctly calculated for a circle with very small radius
    */

    [Fact]
    public void GivenCircle_WhenConstructed_ThenBoundingBoxIsCorrect()
    {
        // Arrange
        float radius = 5.0f;
        var circle = new Circle(radius, Color.Red, new MockShapeDrawer());

        // Act
        var boundingBox = circle.BoundingBox;

        // Assert
        boundingBox.TopLeft.Should().Be(new PositionalVector2(-radius, -radius));
        boundingBox.BottomRight.Should().Be(new PositionalVector2(radius, radius));
    }

    [Fact]
    public void GivenCircle_WhenConstructedWithSmallRadius_ThenBoundingBoxIsCorrect()
    {
        // Arrange
        float radius = 0.0001f;
        var circle = new Circle(radius, Color.Red, new MockShapeDrawer());

        // Act
        var boundingBox = circle.BoundingBox;

        // Assert
        boundingBox.TopLeft.Should().Be(new PositionalVector2(-radius, -radius));
        boundingBox.BottomRight.Should().Be(new PositionalVector2(radius, radius));
    }
    #endregion


    #region Contains Tests
    /*
        - Test that point within zero centered circle yields true
        - Test that point within offset circle yields true
        - Test that point on zero centered circle yields true
        - Test that point on offset circle yields true
        - Test that point outside zero centered circle yields false
        - Test that point outside offset circle yields false
    */

    [Theory]
    [InlineData(1f, -2f, true)]  // Inside circle
    [InlineData(5f,  0f, true)]  // On circle
    [InlineData(7f,  0f, false)] // Outside circle
    [InlineData(4f,  4f, false)] // Outside circle, but inside boundingbox
    public void GivenCircle_WhenTestingContains_ThenReturnsExpectedResult(float pointX, float pointY, bool expected)
    {
        // Arrange
        var circle = new Circle(5.0f, Color.Red, new MockShapeDrawer());
        var point = new PositionalVector2(pointX, pointY);

        // Act
        bool result = circle.Contains(point);

        // Assert
        result.Should().Be(expected);
    }

    #endregion


    #region Draw Tests
    /*
    - Test that injected mock is called when drawing a circle
    - Test that invalid position (NaN or Infinity) throws ArgumentException
    - Test that null drawer throws ArgumentNullException
    */

    [Fact]
    public void GivenCircle_WhenDrawCalled_ThenDrawCircleIsCalled()
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        var circle = new Circle(5.0f, Color.Red, mockShapeDrawer);
        var vector = PositionalVector2.Zero;

        // Act
        circle.Draw(vector);

        // Assert
        mockShapeDrawer.DrawCalled.Should().BeTrue();
    }

    #endregion
}