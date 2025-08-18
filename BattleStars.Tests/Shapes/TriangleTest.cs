using BattleStars.Shapes;
using System.Drawing;
using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests.Shapes;

public class TriangleTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(Vector2 v1, Vector2 v2, Color color) { }
        public void DrawTriangle(Vector2 Point1, Vector2 Point2, Vector2 Point3, Color color)
        {
            DrawCalled = true;
        }
        public void DrawCircle(Vector2 center, float radius, Color color) { }
    }

    #region Constructor Tests
    /*
        Tests for the Triangle constructor.
        - Validates that the constructor throws an ArgumentException when the points contain NaN.
        - Validates that the constructor throws an ArgumentException when the points contain infinity.
        - Validates that the constructor throws an ArgumentException when the points do not form a valid triangle.
        - Validates that a Triangle can be constructed with valid points.

    */

    [Theory]
    [InlineData(float.NaN, 0f, 0f, 1f, 1f, 2f, "point1.X")]
    [InlineData(0f, float.NaN, 0f, 1f, 1f, 2f, "point1.Y")]
    [InlineData(0f, 0f, float.NaN, 1f, 1f, 2f, "point2.X")]
    [InlineData(0f, 0f, 1f, float.NaN, 1f, 2f, "point2.Y")]
    [InlineData(0f, 0f, 1f, 1f, float.NaN, 2f, "point3.X")]
    [InlineData(0f, 0f, 1f, 1f, 2f, float.NaN, "point3.Y")]
    [InlineData(float.PositiveInfinity, 0f, 0f, 1f, 1f, 2f, "point1.X")]
    [InlineData(0f, float.NegativeInfinity, 0f, 1f, 1f, 2f, "point1.Y")]
    [InlineData(0f, 0f, float.NegativeInfinity, 1f, 1f, 2f, "point2.X")]
    [InlineData(0f, 0f, 1f, float.PositiveInfinity, 1f, 2f, "point2.Y")]
    [InlineData(0f, 0f, 1f, 1f, float.PositiveInfinity, 2f, "point3.X")]
    [InlineData(0f, 0f, 1f, 1f, 2f, float.NegativeInfinity, "point3.Y")]
    public void GivenInvalidCorner_WhenConstructingTriangle_ThenThrowsArgumentException(float point1x, float point1y, float point2x, float point2y, float point3x, float point3y, string paramName)
    {
        var point1 = new Vector2(point1x, point1y);
        var point2 = new Vector2(point2x, point2y);
        var point3 = new Vector2(point3x, point3y);
        Action act = () => new Triangle(point1, point2, point3, Color.Red);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    [Theory]
    [InlineData(1f, 1f, 1f, 1f, 1f, 1f)] // All points the same
    [InlineData(0f, 0f, 0f, 0f, 1f, 1f)] // Point1 == Point2
    [InlineData(0f, 0f, 1f, 1f, 0f, 0f)] // Point1 == Point3
    [InlineData(1f, 1f, 0f, 0f, 0f, 0f)] // Point2 == Point3
    [InlineData(0f, 0f, 1f, 0f, 2f, 0f)] // All Points on a line
    public void GivenThreePoints_WhenTheyDoNotFormTriangle_ThenThrowsArgumentException(float Point1x, float Point1y, float Point2x, float Point2y, float Point3x, float Point3y)
    {
        var Point1 = new Vector2(Point1x, Point1y);
        var Point2 = new Vector2(Point2x, Point2y);
        var Point3 = new Vector2(Point3x, Point3y);
        Action act = () => new Triangle(Point1, Point2, Point3, Color.Red);

        act.Should().Throw<ArgumentException>()
            .WithMessage("The points do not form a valid triangle.*");
    }

    [Fact]
    public void GivenValidCorners_WhenConstructingTriangle_ThenSetsProperties()
    {
        var Point1 = new Vector2(0, 0);
        var Point2 = new Vector2(1, 0);
        var Point3 = new Vector2(0, 1);
        var color = Color.Blue;
        var tri = new Triangle(Point1, Point2, Point3, color);

        tri.Point1.Should().Be(Point1);
        tri.Point2.Should().Be(Point2);
        tri.Point3.Should().Be(Point3);
        tri.Color.Should().Be(color);
    }

    #endregion

    #region Contains Tests
    /*
    Tests for the Contains method of the Triangle class.
    - Validates that the Contains method returns true for points inside the triangle.
    - Validates that the Contains method returns false for points outside the triangle.
    - Validates that the Contains method returns true for points on the edges of the triangle.
    - Validates that the Contains method returns true for points on the vertices of the triangle.
    - Validates that the Contains method returns false for points inside the bounding box but outside the triangle.
    - Validates that the Contains method throws an ArgumentException when the point is invalid.
    - Validates that the Contains method throws an ArgumentException when the entity position is invalid.
    */

    [Theory]
    [InlineData(0.2f, 0.2f, 0f, 0f, true)]   // inside, origin
    [InlineData(  0f,   0f, 0f, 0f, true)]   // on vertex, origin
    [InlineData(0.5f,   0f, 0f, 0f, true)]   // on edge, origin
    [InlineData(  2f,   2f, 0f, 0f, false)]  // outside, origin
    [InlineData(0.8f, 0.8f, 0f, 0f, false)]  // Inside Bounding Box, but outside Triangle
    [InlineData(1.2f, 1.2f, 1f, 1f, true)]   // inside, offset
    [InlineData(  1f,   1f, 1f, 1f, true)]   // on vertex, offset
    [InlineData(1.5f,   1f, 1f, 1f, true)]   // on edge, offset
    [InlineData(  3f,   3f, 1f, 1f, false)]  // outside, offset
    [InlineData(1.8f, 1.8f, 1f, 1f, false)]  // Inside Bounding Box, but outside Triangle
    public void GivenTriangle_WhenTestingContains_ThenReturnsExpected(float pointX, float pointY, float entityX, float entityY, bool expected)
    {
        var Point1 = new Vector2(0, 0);
        var Point2 = new Vector2(1, 0);
        var Point3 = new Vector2(0, 1);
        var tri = new Triangle(Point1, Point2, Point3, Color.Red);
        var entityPos = new Vector2(entityX, entityY);
        var point = new Vector2(pointX, pointY);

        tri.Contains(point, entityPos).Should().Be(expected);
    }

    [Theory]
    [InlineData(float.NaN, 0f, 0f, 0f, "point.X")]
    [InlineData(0f, float.NaN, 0f, 0f, "point.Y")]
    [InlineData(float.PositiveInfinity, 0f, 0f, 0f, "point.X")]
    [InlineData(0f, float.NegativeInfinity, 0f, 0f, "point.Y")]
    [InlineData(0f, 0f, float.NaN, 0f, "entityPosition.X")]
    [InlineData(0f, 0f, 0f, float.NaN, "entityPosition.Y")]
    [InlineData(0f, 0f, float.PositiveInfinity, 0f, "entityPosition.X")]
    [InlineData(0f, 0f, 0f, float.NegativeInfinity, "entityPosition.Y")]
    public void GivenTriangle_WhenTestingContains_WithInvalidPointOrEntity_ThenThrowsArgumentException(float px, float py, float ex, float ey, string paramName)
    {
        var tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), Color.Red);
        var point = new Vector2(px, py);
        var entityPos = new Vector2(ex, ey);

        Action act = () => tri.Contains(point, entityPos);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    #endregion

    #region Draw Tests
    /*
        Tests for the Draw method of the Triangle class.
        - Validates that the Draw method calls the drawer.
        - Validates that the Draw method throws an ArgumentNullException when the drawer is null.
        - Validates that the Draw method throws an ArgumentException when the position is NaN or Infinity.
    */

    [Fact]
    public void GivenTriangle_WhenDrawCalled_ThenDrawerIsCalled()
    {
        var drawer = new MockShapeDrawer();
        var tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), Color.Red);
        tri.Draw(new Vector2(1, 1), drawer);

        drawer.DrawCalled.Should().BeTrue();
    }

    [Theory]
    [InlineData(float.NaN, 0f, "entityPosition.X")]
    [InlineData(0f, float.NaN, "entityPosition.Y")]
    [InlineData(float.PositiveInfinity, 0f, "entityPosition.X")]
    [InlineData(0f, float.NegativeInfinity, "entityPosition.Y")]
    public void GivenTriangle_WhenDrawCalled_WithInvalidPosition_ThenThrowsArgumentException(float px, float py, string paramName)
    {
        var drawer = new MockShapeDrawer();
        var tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), Color.Red);
        var position = new Vector2(px, py);

        Action act = () => tri.Draw(position, drawer);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    [Fact]
    public void GivenTriangle_WhenDrawCalled_WithNullDrawer_ThenThrowsArgumentNullException()
    {
        var tri = new Triangle(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), Color.Red);
        var position = new Vector2(1, 1);

        Action act = () => tri.Draw(position, null!);

        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("drawer");
    }

    #endregion
}