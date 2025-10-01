using System.Drawing;
using FluentAssertions;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.ValueObjects;

public class ShapeDescriptorTest
{
    [Theory]
    [InlineData(ShapeType.Hexagon)]
    [InlineData(ShapeType.Circle)]
    [InlineData(ShapeType.Triangle)]
    [InlineData(ShapeType.Square)]
    public void GivenValidParameters_WhenConstructed_ThenPropertiesAreSet(ShapeType shapeType)
    {
        // Given
        var scale = 2.5f;
        var color = Color.Blue;

        // When
        var desc = new ShapeDescriptor(shapeType, scale, color);

        // Then
        desc.ShapeType.Should().Be(shapeType);
        desc.Scale.Should().Be(scale);
        desc.Color.Should().Be(color);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInvalidScale_WhenConstructed_ThenThrowsArgumentException(float invalidScale)
    {
        // Given
        var shapeType = ShapeType.Circle;
        var color = Color.Red;

        // When
        Action act = () => new ShapeDescriptor(shapeType, invalidScale, color);

        // Then
        act.Should().Throw<ArgumentException>();
    }
}