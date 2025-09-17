using System.Drawing;
using FluentAssertions;
using BattleStars.Shapes;
using BattleStars.Utility;
using Moq;

namespace BattleStars.Tests.Shapes;

public class ShapeFactoryTest
{
    [Fact]
    public void GivenNullDrawer_WhenConstructed_ThenThrowsArgumentNullException()
    {
        // Given, When
        Action act = () => new ShapeFactory(null!);

        // Then
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShapeDescriptor_WhenCreateShape_ThenThrowsArgumentNullException()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);

        // When
        Action act = () => factory.CreateShape(null!);

        // Then
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(ShapeType.Circle, "Circle")]
    [InlineData(ShapeType.Square, "Rectangle")]
    [InlineData(ShapeType.Triangle, "Triangle")]
    [InlineData(ShapeType.Hexagon, "PolyShape")]
    public void GivenValidShapeDescriptor_WhenCreateShape_ThenReturnsCorrectShapeType(ShapeType shapeType, string ExpectedName)
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var color = Color.Red;
        var size = 2.0f;
        var descriptor = new ShapeDescriptor(shapeType, size, color);

        // When
        var shape = factory.CreateShape(descriptor);

        // Then
        shape.Should().NotBeNull();
        shape.GetType().Name.Should().Contain(ExpectedName);
    }

    [Fact]
    public void GivenInvalidShapeType_WhenCreateShape_ThenThrowsArgumentException()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var invalidShapeType = (ShapeType)999;
        var descriptor = new ShapeDescriptor(invalidShapeType, 1.0f, Color.Red);

        // When
        Action act = () => factory.CreateShape(descriptor);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenCircleDescriptor_WhenCreateShape_ThenCircleBehavesCorrectly()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var descriptor = new ShapeDescriptor(ShapeType.Circle, 3.0f, Color.Green);

        // When
        var shape = factory.CreateShape(descriptor);

        // Then
        shape.Should().BeOfType<Circle>();
        var circle = (Circle)shape;
        circle.BoundingBox.TopLeft.X.Should().BeApproximately(-3.0f, 0.0001f);
        circle.BoundingBox.TopLeft.Y.Should().BeApproximately(-3.0f, 0.0001f);
        circle.BoundingBox.BottomRight.X.Should().BeApproximately(3.0f, 0.0001f);
        circle.BoundingBox.BottomRight.Y.Should().BeApproximately(3.0f, 0.0001f);
    }

    [Fact]
    public void GivenSquareDescriptor_WhenCreateShape_ThenSquareHasCorrectCornersAndColor()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var descriptor = new ShapeDescriptor(ShapeType.Square, 4.0f, Color.Blue);

        // When
        var shape = factory.CreateShape(descriptor);

        // Then
        shape.Should().BeOfType<BattleStars.Shapes.Rectangle>();
        var rect = (BattleStars.Shapes.Rectangle)shape;
        rect.BoundingBox.Should().NotBeNull();
        rect.BoundingBox.TopLeft.X.Should().BeApproximately(-2.0f, 0.0001f);
        rect.BoundingBox.TopLeft.Y.Should().BeApproximately(-2.0f, 0.0001f);
        rect.BoundingBox.BottomRight.X.Should().BeApproximately(2.0f, 0.0001f);
        rect.BoundingBox.BottomRight.Y.Should().BeApproximately(2.0f, 0.0001f);
        rect.Color.Should().Be(Color.Blue);
    }

    [Fact]
    public void GivenTriangleDescriptor_WhenCreateShape_ThenTriangleHasCorrectPointsAndColor()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var scale = 2.0f;
        var _defaultSize = 1.0f;
        var descriptor = new ShapeDescriptor(ShapeType.Triangle, scale, Color.Yellow);

        // When
        var shape = factory.CreateShape(descriptor);

        // Then
        shape.Should().BeOfType<Triangle>();
        var triangle = (Triangle)shape;
        triangle.Color.Should().Be(Color.Yellow);
        // Points are calculated, so just check they are not default

        float halfSize = scale * _defaultSize / 2f;
        float height = (float)(Math.Sqrt(3) * halfSize);

        // Center the centroid at (0,0)
        PositionalVector2 point1 = new(-halfSize, height / 3f);
        PositionalVector2 point2 = new(halfSize, height / 3f);
        PositionalVector2 point3 = new(0, -2f * height / 3f);

        triangle.Point1.Should().Be(point1);
        triangle.Point2.Should().Be(point2);
        triangle.Point3.Should().Be(point3);
    }

    [Fact]
    public void GivenHexagonDescriptor_WhenCreateShape_ThenPolyShapeHasSixTriangles()
    {
        // Given
        var drawer = new Mock<IShapeDrawer>().Object;
        var factory = new ShapeFactory(drawer);
        var descriptor = new ShapeDescriptor(ShapeType.Hexagon, 6.0f, Color.Purple);

        // When
        var shape = factory.CreateShape(descriptor);

        // Then
        shape.Should().BeOfType<PolyShape>();
        var poly = (PolyShape)shape;
        poly.BoundingBox.Should().NotBeNull();
        poly.BoundingBox.TopLeft.X.Should().BeApproximately(-3f, 0.0001f);
        poly.BoundingBox.TopLeft.Y.Should().BeApproximately(-2.598f, 0.0001f);
        poly.BoundingBox.BottomRight.X.Should().BeApproximately(3f, 0.0001f);
        poly.BoundingBox.BottomRight.Y.Should().BeApproximately(2.598f, 0.0001f);
    }
}