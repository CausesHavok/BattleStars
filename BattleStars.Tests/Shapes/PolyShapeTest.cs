using BattleStars.Shapes;
using System.Drawing;
using System.Numerics;
using FluentAssertions;
using BattleStars.Utility;

namespace BattleStars.Tests.Shapes;

public class PolyShapeTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public int TimesCalled { get; private set; }
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color) { }
        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color)
        {
            TimesCalled++;
            DrawCalled = true;
        }
        public void DrawCircle(PositionalVector2 center, float radius, Color color) { }
    }

    #region Constructor Tests
    /*
        Tests for the Polygon constructor.
        - Ensure that the polygon is not null.
        - Ensure that the triangles array is not null or empty.
        - Ensure that malformed triangles are not allowed.
        - Ensure that polygons of one triangle are allowed.
        - Ensure that polygons of multiple triangles are allowed.
    */
    [Fact]
    public void GivenNullTriangleArray_WhenConstructingPolygon_ThenThrowsArgumentException()
    {
        Action act = () => new PolyShape(null!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("A polygon must have at least one shape.*");
    }

    [Fact]
    public void GivenEmptyTriangleArray_WhenConstructingPolygon_ThenThrowsArgumentException()
    {
        Action act = () => new PolyShape(Array.Empty<Triangle>());

        act.Should().Throw<ArgumentException>()
            .WithMessage("A polygon must have at least one shape.*");
    }

    [Fact]
    public void GivenMalformedTriangle_WhenConstructingPolygon_ThenThrowsArgumentException()
    {
        Action act = () => new PolyShape(
        [
            new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red),
            new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red),
            new Triangle(new PositionalVector2(0, 0), new PositionalVector2(0, 0), new PositionalVector2(0, 0), Color.Red) // Malformed triangle
        ]);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenSingleTriangle_WhenConstructingPolygon_ThenDoesNotThrowArgumentException()
    {
        var triangle = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        Action act = () => new PolyShape([triangle]);

        act.Should().NotThrow<ArgumentException>();
    }

    [Fact]
    public void GivenMultipleTriangles_WhenConstructingPolygon_ThenDoesNotThrowArgumentException()
    {
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red);
        Action act = () => new PolyShape([t1, t2]);

        act.Should().NotThrow<ArgumentException>();
    }

    #endregion

    #region Contains Tests
    /*
        Tests for the Contains method of the Polygon class.
        - Ensure that the method returns the expected result for points inside and outside the polygon.
            - Test point within the first triangle.
            - Test point within the second triangle.
            - Test point outside the polygon.
            - Test point on the edge of the polygon.
            - Test point at a vertex of the polygon.
            - Test point within bounding box of the polygon, but outside the triangles.
        - Ensure that the method handles invalid input gracefully.
            - Test with NaN and Infinity for point
            - Test with NaN and Infinity for entity position
    */

    [Theory]
    [InlineData(0.2f,  0.2f, true)]  // Inside first triangle
    [InlineData(0.2f, -0.2f, true)]  // Inside second triangle
    [InlineData(  8f,    8f, false)] // Outside polygon
    [InlineData(  0f,    1f, true)]  // Vertex of first triangle
    [InlineData(  0f,   -1f, true)]  // Vertex of second triangle
    [InlineData(0.5f,    0f, true)]  // Point on edge of polygon
    [InlineData(0.8f,  0.8f, false)] // Inside bounding box but outside triangles
    public void GivenPolygon_WhenTestingContains_ThenReturnsExpected(float pointX, float pointY, bool expected)
    {
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, -1), Color.Red);
        var poly = new PolyShape([t1, t2]);
        var point = new PositionalVector2(pointX, pointY);

        poly.Contains(point).Should().Be(expected);
    }

    [Theory]
    [InlineData(             float.NaN,                     0f, "Position.X")]
    [InlineData(                    0f,              float.NaN, "Position.Y")]
    [InlineData(float.PositiveInfinity,                     0f, "Position.X")]
    [InlineData(                    0f, float.NegativeInfinity, "Position.Y")]
    public void GivenPolygon_WhenTestingContains_WithInvalidPointOrEntity_ThenThrowsArgumentException(float px, float py, string paramName)
    {
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red);
        var poly = new PolyShape([t1, t2]);
        var point = new Vector2(px, py);

        Action act = () => poly.Contains(point);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    #endregion

    #region Draw Tests
    /*
        Tests for the Draw method of the Polygon class.
        - Ensure that when polygon.draw is called, it triggers triangle.draw for each triangle in the polygon.
        - Ensure that the drawer is not null.
        - Ensure that the polygon is not null.
    */

    [Fact]
    public void GivenPolygon_WhenDrawCalled_ThenDrawerIsCalledForEachTriangle()
    {
        var drawer = new MockShapeDrawer();
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red);
        var poly = new PolyShape([t1, t2]);

        poly.Draw(new PositionalVector2(1, 1), drawer);
        var TimesCalled = drawer.TimesCalled;

        drawer.DrawCalled.Should().BeTrue();
        TimesCalled.Should().Be(2);
    }

    [Theory]
    [InlineData(float.NaN,              0f,                     "Position.X")]
    [InlineData(0f,                     float.NaN,              "Position.Y")]
    [InlineData(float.PositiveInfinity, 0f,                     "Position.X")]
    [InlineData(0f,                     float.NegativeInfinity, "Position.Y")]
    public void GivenPolygon_WhenDrawCalled_WithInvalidPosition_ThenThrowsArgumentException(float px, float py, string paramName)
    {
        var drawer = new MockShapeDrawer();
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red);
        var poly = new PolyShape([t1, t2]);
        var position = new Vector2(px, py);

        Action act = () => poly.Draw(position, drawer);

        act.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(paramName);
    }

    [Fact]
    public void GivenPolygon_WhenDrawCalled_WithNullDrawer_ThenThrowsArgumentNullException()
    {
        var t1 = new Triangle(new PositionalVector2(0, 0), new PositionalVector2(1, 0), new PositionalVector2(0, 1), Color.Red);
        var t2 = new Triangle(new PositionalVector2(1, 0), new PositionalVector2(1, 1), new PositionalVector2(0, 1), Color.Red);
        var poly = new PolyShape([t1, t2]);
        var position = new PositionalVector2(1, 1);

        Action act = () => poly.Draw(position, null!);

        act.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("drawer");
    }

    #endregion
}