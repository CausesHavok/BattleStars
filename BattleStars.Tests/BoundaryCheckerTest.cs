using FluentAssertions;
using BattleStars.Logic;

namespace BattleStars.Tests;

public class BoundaryCheckerTest
{
    [Theory]
    [InlineData(float.NaN, 10, 0, 10)]
    [InlineData(0, float.NaN, 0, 10)]
    [InlineData(0, 10, float.NaN, 10)]
    [InlineData(0, 10, 0, float.NaN)]
    [InlineData(float.PositiveInfinity, 10, 0, 10)]
    [InlineData(0, float.NegativeInfinity, 0, 10)]
    [InlineData(0, 10, float.PositiveInfinity, 10)]
    [InlineData(0, 10, 0, float.NegativeInfinity)]
    public void GivenNaNOrInfinity_WhenConstructed_ThenThrowsArgumentException(float minX, float maxX, float minY, float maxY)
    {
        // Given, When
        Action act = () => new BoundaryChecker(minX, maxX, minY, maxY);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(10, 0, 0, 10)]
    [InlineData(0, 10, 10, 0)]
    [InlineData(5, 5, 0, 10)]
    [InlineData(0, 10, 7, 7)]
    public void GivenInvalidRanges_WhenConstructed_ThenThrowsArgumentException(float minX, float maxX, float minY, float maxY)
    {
        // Given, When
        Action act = () => new BoundaryChecker(minX, maxX, minY, maxY);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenNaNOrInfinity_WhenCheckingXBounds_ThenThrowsArgumentException(float x)
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When
        Action act = () => checker.IsOutsideXBounds(x);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenNaNOrInfinity_WhenCheckingYBounds_ThenThrowsArgumentException(float y)
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When
        Action act = () => checker.IsOutsideYBounds(y);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenNaNOrInfinity_WhenCalculatingXDistance_ThenThrowsArgumentException(float x)
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When
        Action act = () => checker.XDistanceToBoundary(x);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenNaNOrInfinity_WhenCalculatingYDistance_ThenThrowsArgumentException(float y)
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When
        Action act = () => checker.YDistanceToBoundary(y);

        // Then
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidBounds_WhenCheckingXAndY_ThenReturnsExpectedResults()
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When, Then
        checker.IsOutsideXBounds(-1).Should().BeTrue();
        checker.IsOutsideXBounds(0).Should().BeFalse();
        checker.IsOutsideXBounds(10).Should().BeFalse();
        checker.IsOutsideXBounds(11).Should().BeTrue();

        checker.IsOutsideYBounds(-1).Should().BeTrue();
        checker.IsOutsideYBounds(0).Should().BeFalse();
        checker.IsOutsideYBounds(10).Should().BeFalse();
        checker.IsOutsideYBounds(11).Should().BeTrue();
    }

    [Fact]
    public void GivenValidBounds_WhenCalculatingDistance_ThenReturnsExpectedResults()
    {
        // Given
        var checker = new BoundaryChecker(0, 10, 0, 10);

        // When, Then
        checker.XDistanceToBoundary(1).Should().Be(1);
        checker.XDistanceToBoundary(9).Should().Be(1);
        checker.XDistanceToBoundary(0).Should().Be(0);
        checker.XDistanceToBoundary(10).Should().Be(0);

        checker.YDistanceToBoundary(1).Should().Be(1);
        checker.YDistanceToBoundary(9).Should().Be(1);
        checker.YDistanceToBoundary(0).Should().Be(0);
        checker.YDistanceToBoundary(10).Should().Be(0);
    }
}