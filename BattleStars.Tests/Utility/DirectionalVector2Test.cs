using System.Numerics;
using FluentAssertions;
using BattleStars.Utility;

namespace BattleStars.Tests.Utility;

public class DirectionalVector2Test
{
    [Fact]
    public void GivenNormalizedVector_WhenConstructed_ThenValueIsSet()
    {
        // Arrange
        var vector = Vector2.Normalize(new Vector2(3, 4));

        // Act
        var directional = new DirectionalVector2(vector);

        // Assert
        directional.Direction.Should().Be(vector);
    }

    [Fact]
    public void GivenZeroVector_WhenConstructed_ThenValueIsSet()
    {
        // Arrange
        var vector = Vector2.Zero;

        // Act
        var directional = new DirectionalVector2(vector);

        // Assert
        directional.Direction.Should().Be(vector);
    }

    [Fact]
    public void GivenNonNormalizedVector_WhenConstructed_ThenThrowsArgumentException()
    {
        // Arrange
        var vector = new Vector2(3, 4); // Not normalized

        // Act
        Action act = () => new DirectionalVector2(vector);

        // Assert
        act.Should().Throw<ArgumentException>();
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
        Action act = () => new DirectionalVector2(vector);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNormalizedVector_WhenValueSet_ThenValueIsUpdated()
    {
        // Arrange
        var directional = new DirectionalVector2(Vector2.UnitX);
        var newVector = Vector2.Normalize(new Vector2(1, 1));

        // Act
        directional.Direction = newVector;

        // Assert
        directional.Direction.Should().Be(newVector);
    }

    [Fact]
    public void GivenNonNormalizedVector_WhenValueSet_ThenThrowsArgumentException()
    {
        // Arrange
        var directional = new DirectionalVector2(Vector2.UnitX);
        var invalidVector = new Vector2(2, 0); // Not normalized

        // Act
        Action act = () => directional.Direction = invalidVector;

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenDirectionalVector2_WhenImplicitlyConvertedToVector2_ThenReturnsValue()
    {
        // Arrange
        var vector = Vector2.UnitY;
        var directional = new DirectionalVector2(vector);

        // Act
        Vector2 result = directional;

        // Assert
        result.Should().Be(vector);
    }

}