using System.Drawing;
using BattleStars.Shapes;
using BattleStars.Utility;
using FluentAssertions;

namespace BattleStars.Tests.Shapes;
public class RaylibShapeDrawerTests
{
    private class LoggingGraphics : IRaylibGraphics
    {
        public List<string> Calls { get; } = new();

        public void DrawRectangle(PositionalVector2 topLeft, PositionalVector2 size, Color color) =>
            Calls.Add($"Rectangle: {topLeft}, {size}, {color}");

        public void DrawTriangle(PositionalVector2 p1, PositionalVector2 p2, PositionalVector2 p3, Color color) =>
            Calls.Add($"Triangle: {p1}, {p2}, {p3}, {color}");

        public void DrawCircle(PositionalVector2 center, float radius, Color color) =>
            Calls.Add($"Circle: {center}, {radius}, {color}");
    }

    [Fact]
    public void GivenNullIRaylibGraphics_WhenConstructed_ThenThrowsNullException()
    {
        Action act = () => _ = new RaylibShapeDrawer(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenRectangle_WhenDrawn_ThenGraphicsReceivesCorrectCall()
    {
        // Arrange
        var logger = new LoggingGraphics();
        var drawer = new RaylibShapeDrawer(logger);
        var topLeft = new PositionalVector2(-10, -20);
        var bottomRight = new PositionalVector2(30, 40);
        var expectedSize = bottomRight - topLeft;
        var color = Color.Red;

        // Act
        drawer.DrawRectangle(topLeft, bottomRight, color);

        // Assert
        logger.Calls.Should().ContainSingle()
            .Which.Should().StartWith("Rectangle:")
            .And.Contain($"{topLeft}")
            .And.Contain($"{expectedSize}")
            .And.Contain($"{color}");
    }

    [Fact]
    public void GivenTriangle_WhenDrawn_ThenGraphicsReceivesCorrectCall()
    {
        // Arrange
        var logger = new LoggingGraphics();
        var drawer = new RaylibShapeDrawer(logger);
        var p1 = new PositionalVector2(0, 0);
        var p2 = new PositionalVector2(10, 0);
        var p3 = new PositionalVector2(5, 10);
        var color = Color.Blue;

        // Act
        drawer.DrawTriangle(p1, p2, p3, color);

        // Assert
        logger.Calls.Should().ContainSingle()
            .Which.Should().StartWith("Triangle:")
            .And.Contain($"{p1}")
            .And.Contain($"{p2}")
            .And.Contain($"{p3}")
            .And.Contain($"{color}");
    }
    [Fact]
    public void GivenCircle_WhenDrawn_ThenGraphicsReceivesCorrectCall()
    {
        // Arrange
        var logger = new LoggingGraphics();
        var drawer = new RaylibShapeDrawer(logger);
        var center = new PositionalVector2(50, 50);
        var radius = 25f;
        var color = Color.Green;

        // Act
        drawer.DrawCircle(center, radius, color);

        // Assert
        logger.Calls.Should().ContainSingle()
            .Which.Should().StartWith("Circle:")
            .And.Contain($"{center}")
            .And.Contain($"{radius}")
            .And.Contain($"{color}");
    }

}