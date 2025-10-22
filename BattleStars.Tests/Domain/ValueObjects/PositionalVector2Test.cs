using System.Numerics;
using FluentAssertions;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.Domain.ValueObjects;

public class PositionalVector2Test
{
    [Fact]
    public void GivenValidVector_WhenConstructed_ThenValueIsSet()
    {
        // Arrange
        var vector = new Vector2(1, 2);

        // Act
        var positional = new PositionalVector2(vector);

        // Assert
        positional.Position.Should().Be(vector);
    }

    [Theory]
    [InlineData(float.NaN, 0)]
    [InlineData(0, float.NaN)]
    [InlineData(float.PositiveInfinity, 0)]
    [InlineData(0, float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity, 0)]
    [InlineData(0, float.NegativeInfinity)]
    public void GivenInvalidVector_WhenConstructed_ThenThrowsArgumentException(float x, float y)
    {
        // Arrange
        var vector = new Vector2(x, y);

        // Act
        Action act = () => new PositionalVector2(vector);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidVector_WhenValueSet_ThenValueIsUpdated()
    {
        // Arrange
        var positional = new PositionalVector2(new Vector2(1, 2));
        var newVector = new Vector2(3, 4);

        // Act
        positional.Position = newVector;

        // Assert
        positional.Position.Should().Be(newVector);
    }

    [Fact]
    public void GivenInvalidVector_WhenValueSet_ThenThrowsArgumentException()
    {
        // Arrange
        var positional = new PositionalVector2(new Vector2(1, 2));
        var invalidVector = new Vector2(float.NaN, 0);

        // Act
        Action act = () => positional.Position = invalidVector;

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPositionalVector2_WhenImplicitlyConvertedToVector2_ThenReturnsValue()
    {
        // Arrange
        var vector = new Vector2(5, 6);
        var positional = new PositionalVector2(vector);

        // Act
        Vector2 result = positional;

        // Assert
        result.Should().Be(vector);
    }

}