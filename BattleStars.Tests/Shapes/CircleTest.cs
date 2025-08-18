using BattleStars.Shapes;
using System.Drawing;
using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests.Shapes;

public class CircleTest
{

    public class MockShapeDrawer : IShapeDrawer
    {
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(Vector2 v1, Vector2 v2, Color color)
        {
            DrawCalled = true;
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            DrawCalled = true;
        }

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            DrawCalled = true;
        }
    }

    #region Constructor Tests
    /*
        - Test the Circle constructor with valid parameters
        - Test the Circle constructor with invalid parameters
        - Test the Circle constructor with NaN, Infinity, negative, and zero radius
    */

    [Theory]
    [InlineData(float.NaN, "NaN")]
    [InlineData(float.PositiveInfinity, "infinity")]
    [InlineData(float.NegativeInfinity, "infinity")]
    public void GivenCircle_WhenConstructor_WithInvalidRadius_ThenThrowsArgumentException(float radius, string expectedException)
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        Action act = () => new Circle(radius, Color.Red);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("radius cannot be " + expectedException + ".*")
            .And.ParamName.Should().Be("radius");
    }

    [Theory]
    [InlineData(0.0f, "zero")]
    [InlineData(-1.0f, "negative")]
    public void GivenCircle_WhenConstructed_WithZeroOrNegativeRadius_ThenThrowsArgumentOutOfRangeException(float radius, string expectedException)
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        Action act = () => new Circle(radius, Color.Red);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("radius cannot be " + expectedException + ".*")
            .And.ParamName.Should().Be("radius");
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
        - Test that invalid point (NaN or Infinity) throws ArgumentException
        - Test that invalid offset point (NaN or Infinity) throws ArgumentException
    */

    [Theory]
    [InlineData(1f, -2f, 0f, 0f, true)]
    [InlineData(1f, -2f, 2f, 2f, true)]
    [InlineData(5f, 0f, 0f, 0f, true)]
    [InlineData(7f, 0f, 2f, 0f, true)]
    [InlineData(5f, 5f, 0f, 0f, false)]
    [InlineData(5f, 5f, 10f, 10f, false)]
    public void GivenCircle_WhenTestingContains_ThenReturnsExpectedResult(float pointX, float pointY, float circleX, float circleY, bool expected)
    {
        var circle = new Circle(5.0f, Color.Red);
        var point = new Vector2(pointX, pointY);
        var circleCenter = new Vector2(circleX, circleY);

        // Act
        bool result = circle.Contains(point, circleCenter);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(float.NaN, 0f, 0f, 0f, "NaN", "point.X")]
    [InlineData(0f, float.NaN, 0f, 0f, "NaN", "point.Y")]
    [InlineData(float.PositiveInfinity, 0f, 0f, 0f, "Infinity", "point.X")]
    [InlineData(0f, float.PositiveInfinity, 0f, 0f, "Infinity", "point.Y")]
    [InlineData(float.NegativeInfinity, 0f, 0f, 0f, "Infinity", "point.X")]
    [InlineData(0f, float.NegativeInfinity, 0f, 0f, "Infinity", "point.Y")]
    public void GivenCircle_WhenTestingContains_WithInvalidPosition_ThenThrowsArgumentException(float pointX, float pointY, float circleX, float circleY, string expectedException, string paramName)
    {
        var circle = new Circle(5.0f, Color.Red);
        var point = new Vector2(pointX, pointY);
        var circleCenter = new Vector2(circleX, circleY);

        // Act
        Action act = () => circle.Contains(point, circleCenter);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(paramName + " cannot be " + expectedException + ".*")
            .And.ParamName.Should().Be(paramName);
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
        var circle = new Circle(5.0f, Color.Red);
        var vector = new Vector2(0, 0);

        // Act
        circle.Draw(vector, mockShapeDrawer);

        // Assert
        mockShapeDrawer.DrawCalled.Should().BeTrue();
    }

    [Theory]
    [InlineData(float.NaN, "NaN")]
    [InlineData(float.PositiveInfinity, "infinity")]
    [InlineData(float.NegativeInfinity, "infinity")]
    public void GivenCircle_WhenDrawCalled_WithInvalidPosition_ThenThrowsArgumentException(float positionX, string expectedException)
    {
        // Arrange
        var mockShapeDrawer = new MockShapeDrawer();
        var circle = new Circle(5.0f, Color.Red);
        var position = new Vector2(positionX, 0);

        // Act
        Action act = () => circle.Draw(position, mockShapeDrawer);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("position.X cannot be " + expectedException + ".*")
            .And.ParamName.Should().Be("position.X");
    }

    [Fact]
    public void GivenCircle_WhenDrawCalled_WithNullDrawer_ThenThrowsArgumentNullException()
    {
        // Arrange
        var circle = new Circle(5.0f, Color.Red);
        var position = new Vector2(0, 0);

        // Act
        Action act = () => circle.Draw(position, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null.*")
            .And.ParamName.Should().Be("drawer");
    }

    #endregion


}