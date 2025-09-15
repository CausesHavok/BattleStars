using BattleStars.Shapes;
using System.Drawing;
using System.Numerics;
using FluentAssertions;
using BattleStars.Utility;

namespace BattleStars.Tests.Shapes;

public class RectangleTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color)
        {
            DrawCalled = true;
        }

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) { }
        public void DrawCircle(PositionalVector2 center, float radius, Color color) { }
    }

    #region Constructor Tests
    /*
        Tests for the Rectangle constructor
        - throws an ArgumentException when the corners contain NaN.
        - throws an ArgumentException when the corners contain Infinity.
        - throws an ArgumentException when the rectangle has zero area.
        - initializes the rectangle correctly with valid corners.
    */

    [Theory]
    [InlineData(float.NaN, 0f, 0f, 1f, "Position.X")]
    [InlineData(0f, float.NaN, 0f, 1f, "Position.Y")]
    [InlineData(0f, 0f, float.NaN, 1f, "Position.X")]
    [InlineData(0f, 0f, 1f, float.NaN, "Position.Y")]
    [InlineData(float.PositiveInfinity, 0f, 0f, 1f, "Position.X")]
    [InlineData(0f, float.PositiveInfinity, 0f, 1f, "Position.Y")]
    [InlineData(0f, 0f, float.PositiveInfinity, 1f, "Position.X")]
    [InlineData(0f, 0f, 1f, float.PositiveInfinity, "Position.Y")]
    public void GivenInvalidCorner_WhenConstructingRectangle_ThenThrowsArgumentException(float v1x, float v1y, float v2x, float v2y, string paramName)
    {
        var v1 = new Vector2(v1x, v1y);
        var v2 = new Vector2(v2x, v2y);
        Action act = () => new BattleStars.Shapes.Rectangle(v1, v2, Color.Red);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    [Theory]
    [InlineData(0f, 0f, 0f, 1f)] // zero width
    [InlineData(0f, 0f, 1f, 0f)] // zero height
    [InlineData(1f, 1f, 1f, 1f)] // zero width and height
    public void GivenZeroWidthOrHeight_WhenConstructingRectangle_ThenThrowsArgumentException(float v1x, float v1y, float v2x, float v2y)
    {
        var v1 = new Vector2(v1x, v1y);
        var v2 = new Vector2(v2x, v2y);
        Action act = () => new BattleStars.Shapes.Rectangle(v1, v2, Color.Red);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle must have non-zero width and height.*");
    }

    [Fact]
    public void GivenValidCorners_WhenConstructingRectangle_ThenSetsProperties()
    {
        var v1 = new Vector2(0, 0);
        var v2 = new Vector2(2, 3);
        var color = Color.Blue;
        var rect = new BattleStars.Shapes.Rectangle(v1, v2, color);

        rect.BoundingBox.TopLeft.Should().Be(new Vector2(0, 0));
        rect.BoundingBox.BottomRight.Should().Be(new Vector2(2, 3));
        rect.Color.Should().Be(color);
    }

    #endregion

    #region Contains Tests
    /*
        Tests for the Rectangle.Contains method
        - returns true for points inside the rectangle.
        - returns true for points on the edge of the rectangle.
        - returns true for points on the corner of the rectangle.
        - returns false for points outside the rectangle.
        - returns true for the origin point.
        - returns true for points inside the rectangle with an offset.
        - returns false for points outside the rectangle with an offset.
    */

    [Theory]
    [InlineData(0.5f, 0.5f, true)]  // inside
    [InlineData(1f, 0f, true)]  // on edge
    [InlineData(1f, 1f, true)]  // on corner
    [InlineData(3f, 3f, false)] // outside
    [InlineData(0f, 0f, true)]  // point on origin
    public void GivenRectangle_WhenTestingContains_ThenReturnsExpected(float pointX, float pointY, bool expected)
    {
        var vec1 = new Vector2(-1, -1);
        var vec2 = new Vector2(1, 1);
        var rect = new BattleStars.Shapes.Rectangle(vec1, vec2, Color.Red);
        var point = new Vector2(pointX, pointY);

        rect.Contains(point).Should().Be(expected);
    }

    [Theory]
    [InlineData(float.NaN, 0f, "Position.X")]
    [InlineData(0f, float.NaN, "Position.Y")]
    [InlineData(float.PositiveInfinity, 0f, "Position.X")]
    [InlineData(0f, float.NegativeInfinity, "Position.Y")]

    public void GivenRectangle_WhenTestingContains_WithInvalidPointOrEntity_ThenThrowsArgumentException(float px, float py, string paramName)
    {
        var rect = new BattleStars.Shapes.Rectangle(new Vector2(0, 0), new Vector2(2, 2), Color.Red);
        var point = new Vector2(px, py);

        Action act = () => rect.Contains(point);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    #endregion

    #region Draw Tests
    /*
        Tests for the Rectangle.Draw method
        - Validates that the method calls the drawer's Draw method with the correct parameters.
        - Validates that the method throws an ArgumentException for invalid positions.
        - Validates that the method throws an ArgumentNullException for null drawers.
    */

    [Fact]
    public void GivenRectangle_WhenDrawCalled_ThenDrawerIsCalled()
    {
        var drawer = new MockShapeDrawer();
        var rect = new BattleStars.Shapes.Rectangle(new Vector2(0, 0), new Vector2(2, 2), Color.Red);
        rect.Draw(new Vector2(1, 1), drawer);

        drawer.DrawCalled.Should().BeTrue();
    }

    [Theory]
    [InlineData(float.NaN, 0f, "Position.X")]
    [InlineData(0f, float.NaN, "Position.Y")]
    [InlineData(float.PositiveInfinity, 0f, "Position.X")]
    [InlineData(0f, float.PositiveInfinity, "Position.Y")]
    public void GivenRectangle_WhenDrawCalled_WithInvalidPosition_ThenThrowsArgumentException(float px, float py, string paramName)
    {
        var drawer = new MockShapeDrawer();
        var rect = new BattleStars.Shapes.Rectangle(new Vector2(0, 0), new Vector2(2, 2), Color.Red);
        var position = new Vector2(px, py);

        Action act = () => rect.Draw(position, drawer);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    [Fact]
    public void GivenRectangle_WhenDrawCalled_WithNullDrawer_ThenThrowsArgumentNullException()
    {
        var rect = new BattleStars.Shapes.Rectangle(new Vector2(0, 0), new Vector2(2, 2), Color.Red);
        var position = new Vector2(1, 1);

        Action act = () => rect.Draw(position, null!);

        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("drawer");
    }

    #endregion
}