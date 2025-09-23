using System.Drawing;
using BattleStars.Shapes;
using BattleStars.Utility;
using FluentAssertions;

namespace BattleStars.Tests.Shapes;

public class ShapeFactoryTest
{
    public class MockShapeDrawer : IShapeDrawer
    {
        public void DrawRectangle(PositionalVector2 v1, PositionalVector2 v2, Color color) { }
        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) { }
        public void DrawCircle(PositionalVector2 center, float radius, Color color) { }
    }

    public class TestShapeDescriptor : IShapeDescriptor
    {
        public ShapeType ShapeType { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }
    }

    [Fact]
    public void GivenNullShapeDescriptor_WhenCreateShapeIsCalled_ThenThrowsArgumentNullException()
    {
        var drawer = new MockShapeDrawer();
        Action act = () => ShapeFactory.CreateShape(null!, drawer);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNullShapeDrawer_WhenCreateShapeIsCalled_ThenThrowsArgumentNullException()
    {
        var desc = new TestShapeDescriptor { ShapeType = ShapeType.Circle, Scale = 1f, Color = Color.Red };
        Action act = () => ShapeFactory.CreateShape(desc, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidScale_WhenCreateShapeIsCalled_ThenThrowsArgumentException(float scale)
    {
        var drawer = new MockShapeDrawer();
        foreach (ShapeType type in Enum.GetValues<ShapeType>())
        {
            var desc = new TestShapeDescriptor { ShapeType = type, Scale = scale, Color = Color.Red };
            Action act = () => ShapeFactory.CreateShape(desc, drawer);
            act.Should().Throw<ArgumentException>($"ShapeType {type} with scale {scale} should throw");
        }
    }

    [Fact]
    public void GivenInvalidShapeType_WhenCreateShapeIsCalled_ThenThrowsArgumentException()
    {
        var drawer = new MockShapeDrawer();
        var desc = new TestShapeDescriptor { ShapeType = (ShapeType)999, Scale = 1f, Color = Color.Red };
        Action act = () => ShapeFactory.CreateShape(desc, drawer);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(ShapeType.Circle)]
    [InlineData(ShapeType.Square)]
    [InlineData(ShapeType.Triangle)]
    [InlineData(ShapeType.Hexagon)]
    public void GivenValidShapeType_WhenCreateShapeIsCalled_ThenReturnsShape(ShapeType type)
    {
        var drawer = new MockShapeDrawer();
        var desc = new TestShapeDescriptor { ShapeType = type, Scale = 1f, Color = Color.Red };
        var shape = ShapeFactory.CreateShape(desc, drawer);
        shape.Should().NotBeNull();
    }
}