using System.Numerics;
using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class GuardTest
{
    [Fact]
    public void GivenNonNullReference_WhenNotNull_ThenReturnsSameInstance()
    {
        // Given
        var sut = new object();

        // When
        var result = Guard.NotNull(sut, nameof(sut));

        // Then
        result.Should().BeSameAs(sut);
    }

    [Fact]
    public void GivenNullReference_WhenNotNull_ThenThrowsArgumentNullException()
    {
        // Given
        object? sut = null;

        // When
        Action Act = () => Guard.NotNull(sut, nameof(sut));

        // Then
        Act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenValidFloat_WhenRequireNotNaN_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenNaN_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        // Given
        var value = float.NaN;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenFiniteFloat_WhenRequireFinite_ThenDoesNotThrow()
    {
        // Given
        var value = 2f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireFinite(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenInfinity_WhenRequireFinite_ThenThrowsArgumentException()
    {
        // Given
        var value = float.PositiveInfinity;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireFinite(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNonNegative_WhenRequireNonNegative_ThenDoesNotThrow()
    {
        // Given
        var value = 0f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonNegative(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenNegative_WhenRequireNonNegative_ThenThrowsArgumentException()
    {
        // Given
        var value = -1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonNegative(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNonZero_WhenRequireNonZero_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonZero(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenZero_WhenRequireNonZero_ThenThrowsArgumentException()
    {
        // Given
        var value = 0f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonZero(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenPositive_WhenRequirePositive_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequirePositive(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenNonPositive_WhenRequirePositive_ThenThrowsArgumentException()
    {
        // Given
        var value = 0f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequirePositive(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidFloat_WhenRequireValid_ThenDoesNotThrow()
    {
        // Given
        var value = 3.14f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireValid(value, nameof(value)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenInvalidFloat_WhenRequireValid_ThenThrowsArgumentException()
    {
        // Given
        var value = float.NaN;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireValid(value, nameof(value)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidVector_WhenRequireNotNaN_ThenDoesNotThrow()
    {
        // Given
        var v = new Vector2(1f, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(v, nameof(v)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenVectorWithNaN_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        // Given
        var v = new Vector2(float.NaN, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(v, nameof(v)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenFiniteVector_WhenRequireFinite_ThenDoesNotThrow()
    {
        // Given
        var v = new Vector2(1f, 2f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireFinite(v, nameof(v)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenInfiniteVector_WhenRequireFinite_ThenThrowsArgumentException()
    {
        // Given
        var v = new Vector2(float.PositiveInfinity, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireFinite(v, nameof(v)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNonZeroVector_WhenRequireNonZero_ThenDoesNotThrow()
    {
        // Given
        var v = new Vector2(1f, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonZero(v, nameof(v)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenZeroVector_WhenRequireNonZero_ThenThrowsArgumentException()
    {
        // Given
        var v = Vector2.Zero;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonZero(v, nameof(v)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNormalizedVector_WhenRequireNormalized_ThenDoesNotThrow()
    {
        // Given
        var v = Vector2.UnitX; // already normalized

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNormalized(v, nameof(v)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenNonNormalizedVector_WhenRequireNormalized_ThenThrowsArgumentException()
    {
        // Given
        var v = new Vector2(2f, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNormalized(v, nameof(v)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenValidVector_WhenRequireValid_ThenDoesNotThrow()
    {
        // Given
        var v = Vector2.UnitY;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireValid(v, nameof(v)))
            .Should().NotThrow();
    }

    [Fact]
    public void GivenInvalidVector_WhenRequireValid_ThenThrowsArgumentException()
    {
        // Given
        var v = new Vector2(float.NaN, 1f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireValid(v, nameof(v)))
            .Should().Throw<ArgumentException>();
    }
}