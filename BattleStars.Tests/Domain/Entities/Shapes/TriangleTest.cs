using System.Drawing;
using FluentAssertions;
using BattleStars.Domain.Entities.Shapes;
using BattleStars.Domain.ValueObjects;
using BattleStars.Presentation.Drawers;

namespace BattleStars.Tests.Entities.Shapes;

public class TriangleTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public bool DrawCalled { get; private set; }

        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color) { }
        public void DrawTriangle(PositionalVector2 Point1, PositionalVector2 Point2, PositionalVector2 Point3, Color color)
        {
            DrawCalled = true;
        }
        public void DrawCircle(PositionalVector2 center, float radius, Color color) { }
    }

    #region Constructor Tests
    /*
        Tests for the Triangle constructor.
        - Validates that the constructor throws an ArgumentException when the points do not form a valid triangle.
        - Validates that a Triangle can be constructed with valid points.

    */

    [Theory]
    [InlineData(1f, 1f, 1f, 1f, 1f, 1f)] // All points the same
    [InlineData(0f, 0f, 0f, 0f, 1f, 1f)] // Point1 == Point2
    [InlineData(0f, 0f, 1f, 1f, 0f, 0f)] // Point1 == Point3
    [InlineData(1f, 1f, 0f, 0f, 0f, 0f)] // Point2 == Point3
    [InlineData(0f, 0f, 1f, 0f, 2f, 0f)] // All Points on a line
    public void GivenThreePoints_WhenTheyDoNotFormTriangle_ThenThrowsArgumentException(float Point1x, float Point1y, float Point2x, float Point2y, float Point3x, float Point3y)
    {
        var Point1 = new PositionalVector2(Point1x, Point1y);
        var Point2 = new PositionalVector2(Point2x, Point2y);
        var Point3 = new PositionalVector2(Point3x, Point3y);
        var drawerMock = new MockShapeDrawer();
        Action act = () => new Triangle(Point1, Point2, Point3, Color.Red, drawerMock);

        act.Should().Throw<ArgumentException>()
            .WithMessage("The points do not form a valid triangle.*");
    }

    [Fact]
    public void GivenValidCorners_WhenConstructingTriangle_ThenSetsProperties()
    {
        var Point1 = PositionalVector2.Zero;
        var Point2 = PositionalVector2.UnitX;
        var Point3 = PositionalVector2.UnitY;
        var color = Color.Blue;
        var drawerMock = new MockShapeDrawer();
        var tri = new Triangle(Point1, Point2, Point3, color, drawerMock);

        tri.Point1.Should().Be(Point1);
        tri.Point2.Should().Be(Point2);
        tri.Point3.Should().Be(Point3);
        tri.Color.Should().Be(color);
    }

    [Fact]
    public void GivenNullDrawer_WhenConstructing_ThenThrowsNullArgumentException()
    {
        var Point1 = new PositionalVector2(1, 0);
        var Point2 = new PositionalVector2(0, 1);
        var Point3 = new PositionalVector2(0, 0);
        Action act = () => new Triangle(Point1, Point2, Point3, Color.Red, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*drawer*");
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
    */

    [Theory]
    [InlineData(0.2f, 0.2f, true)]   // inside
    [InlineData(0f, 0f, true)]   // on vertex
    [InlineData(0.5f, 0f, true)]   // on edge
    [InlineData(2f, 2f, false)]  // outside
    [InlineData(0.8f, 0.8f, false)]  // Inside Bounding Box, but outside Triangle
    public void GivenTriangle_WhenTestingContains_ThenReturnsExpected(float pointX, float pointY, bool expected)
    {
        var Point1 = PositionalVector2.Zero;
        var Point2 = PositionalVector2.UnitX;
        var Point3 = PositionalVector2.UnitY;
        var drawerMock = new MockShapeDrawer();
        var tri = new Triangle(Point1, Point2, Point3, Color.Red, drawerMock);
        var point = new PositionalVector2(pointX, pointY);

        tri.Contains(point).Should().Be(expected);
    }

    #endregion

    #region Draw Tests
    /*
        Tests for the Draw method of the Triangle class.
        - Validates that the Draw method calls the drawer.
        - Validates that the Draw method throws an ArgumentNullException when the drawer is null.
    */

    [Fact]
    public void GivenTriangle_WhenDrawCalled_ThenDrawerIsCalled()
    {
        var drawer = new MockShapeDrawer();
        var tri = new Triangle(PositionalVector2.Zero, PositionalVector2.UnitX, PositionalVector2.UnitY, Color.Red, drawer);
        tri.Draw(new PositionalVector2(1, 1));

        drawer.DrawCalled.Should().BeTrue();
    }

    #endregion

    #region Barycentric Tests
    /*
    Tests for the Contains method of the Triangle class, focusing on the barycentric coordinate branches.
    - Validates that the Contains method returns false for points that trigger each branch of the barycentric coordinate checks (u < 0, v < 0, u + v > 1).
    */

    [Theory]
    [InlineData(1.0f, 2.0f)]  // u < 0
    [InlineData(1.0f, -1.0f)]  // v < 0
    [InlineData(3.0f, 1.0f)]   // u + v > 1
    public void GivenTriangle_WhenTestingAllBarycentricBranches_ThenReturnsExpected(float pointX, float pointY)
    {
        var Point1 = new PositionalVector2(0, 0);
        var Point2 = new PositionalVector2(2, 3);
        var Point3 = new PositionalVector2(4, -2);
        var drawerMock = new MockShapeDrawer();
        var tri = new Triangle(Point1, Point2, Point3, Color.Red, drawerMock);
        var point = new PositionalVector2(pointX, pointY);

        tri.Contains(point).Should().BeFalse();
    }
    
    [Fact]
    public void GivenNearlyColinearTriangleAndColinearTestPointInsideBoundingBox_WhenCallingContains_ThenReturnsFalseDueToZeroDenom()
    {
        var Point1 = new PositionalVector2(0f, 0f);
        var Point2 = new PositionalVector2(1f, 1f);
        var Point3 = new PositionalVector2(2f, 2.000001f); // Slight offset
        var drawerMock = new MockShapeDrawer();
        var tri = new Triangle(Point1, Point2, Point3, Color.Red, drawerMock);

        // This point is inside the bounding box and colinear with the triangle points
        var testPoint = new PositionalVector2(1f, 1f);

        tri.Contains(testPoint).Should().BeFalse(); // Should hit denom == 0
    }

    #endregion
}