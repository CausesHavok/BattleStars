using System.Numerics;
using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class GuardTest
{
    // NotNull<T>
    [Fact]
    public void GivenNonNullReference_WhenNotNull_ThenReturnsSameInstance()
    {
        // Given
        var sut = new object();

        // When
        var result = Guard.NotNull(sut);

        // Then
        result.Should().BeSameAs(sut);
    }

    [Fact]
    public void GivenNullReferenceAndNullName_WhenNotNull_ThenThrowsArgumentNullExceptionWithValueToken()
    {
        // Given
        object? sut = null;

        // When
        Action act = () => Guard.NotNull(sut, null!);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenNullReferenceAndNoName_WhenNotNull_ThenThrowsArgumentNullExceptionWithCallerName()
    {
        // Given
        object? sut = null;

        // When
        Action act = () => Guard.NotNull(sut);

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("sut");
    }

    [Fact]
    public void GivenNullReferenceAndExplicitName_WhenNotNull_ThenThrowsArgumentNullExceptionWithExplicitName()
    {
        // Given
        object? sut = null;

        // When
        Action act = () => Guard.NotNull(sut, "customName");

        // Then
        act.Should().Throw<ArgumentNullException>().WithParameterName("customName");
    }

    // Float guards
    [Fact]
    public void GivenValidFloat_WhenRequireNotNaN_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenNaNAndNullName_WhenRequireNotNaN_ThenThrowsWithValueToken()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireNotNaN(value, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenNaNAndNoName_WhenRequireNotNaN_ThenThrowsWithCallerName()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireNotNaN(value);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void GivenNaNAndExplicitName_WhenRequireNotNaN_ThenThrowsWithExplicitName()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireNotNaN(value, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenFiniteFloat_WhenRequireFinite_ThenDoesNotThrow()
    {
        // Given
        var value = 2f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireFinite(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenInfinityAndNullName_WhenRequireFinite_ThenThrowsWithValueToken()
    {
        // Given
        var value = float.PositiveInfinity;

        // When
        Action act = () => Guard.RequireFinite(value, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenInfinityAndNoName_WhenRequireFinite_ThenThrowsWithCallerName()
    {
        // Given
        var value = float.PositiveInfinity;

        // When
        Action act = () => Guard.RequireFinite(value);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void GivenInfinityAndExplicitName_WhenRequireFinite_ThenThrowsWithExplicitName()
    {
        // Given
        var value = float.PositiveInfinity;

        // When
        Action act = () => Guard.RequireFinite(value, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenNonNegative_WhenRequireNonNegative_ThenDoesNotThrow()
    {
        // Given
        var value = 0f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonNegative(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenNegativeAndNullName_WhenRequireNonNegative_ThenThrowsWithValueToken()
    {
        // Given
        var value = -1f;

        // When
        Action act = () => Guard.RequireNonNegative(value, null!);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenNegativeAndNoName_WhenRequireNonNegative_ThenThrowsWithCallerName()
    {
        // Given
        var value = -1f;

        // When
        Action act = () => Guard.RequireNonNegative(value);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("value");
    }

    [Fact]
    public void GivenNegativeAndExplicitName_WhenRequireNonNegative_ThenThrowsWithExplicitName()
    {
        // Given
        var value = -1f;

        // When
        Action act = () => Guard.RequireNonNegative(value, "custom");

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenNonZero_WhenRequireNonZero_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNonZero(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenZeroAndNullName_WhenRequireNonZero_ThenThrowsWithValueToken()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequireNonZero(value, null!);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenZeroAndNoName_WhenRequireNonZero_ThenThrowsWithCallerName()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequireNonZero(value);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("value");
    }

    [Fact]
    public void GivenZeroAndExplicitName_WhenRequireNonZero_ThenThrowsWithExplicitName()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequireNonZero(value, "custom");

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenPositive_WhenRequirePositive_ThenDoesNotThrow()
    {
        // Given
        var value = 1f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequirePositive(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenNonPositiveAndNullName_WhenRequirePositive_ThenThrowsWithValueToken()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequirePositive(value, null!);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenNonPositiveAndNoName_WhenRequirePositive_ThenThrowsWithCallerName()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequirePositive(value);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("value");
    }

    [Fact]
    public void GivenNonPositiveAndExplicitName_WhenRequirePositive_ThenThrowsWithExplicitName()
    {
        // Given
        var value = 0f;

        // When
        Action act = () => Guard.RequirePositive(value, "custom");

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenValidFloat_WhenRequireValid_ThenDoesNotThrow()
    {
        // Given
        var value = 3.14f;

        // When / Then
        FluentActions.Invoking(() => Guard.RequireValid(value)).Should().NotThrow();
    }

    [Fact]
    public void GivenInvalidFloatAndNullName_WhenRequireValid_ThenThrowsWithValueToken()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireValid(value, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenInvalidFloatAndNoName_WhenRequireValid_ThenThrowsWithCallerName()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireValid(value);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("value");
    }

    [Fact]
    public void GivenInvalidFloatAndExplicitName_WhenRequireValid_ThenThrowsWithExplicitName()
    {
        // Given
        var value = float.NaN;

        // When
        Action act = () => Guard.RequireValid(value, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom");
    }

    // Vector2 guards
    [Fact]
    public void GivenValidVector_WhenRequireNotNaN_ThenDoesNotThrow()
    {
        // Given
        var v = new Vector2(1f, 0f);

        // When / Then
        FluentActions.Invoking(() => Guard.RequireNotNaN(v)).Should().NotThrow();
    }

    [Fact]
    public void GivenVectorWithNaNAndNullName_WhenRequireNotNaN_ThenThrowsForComponentWithValueToken()
    {
        // Given
        var v = new Vector2(float.NaN, 0f);

        // When
        Action act = () => Guard.RequireNotNaN(v, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>.X");
    }

    [Fact]
    public void GivenVectorWithNaNAndNoName_WhenRequireNotNaN_ThenThrowsForComponentWithCallerName()
    {
        // Given
        var v = new Vector2(float.NaN, 0f);

        // When
        Action act = () => Guard.RequireNotNaN(v);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("v.X");
    }

    [Fact]
    public void GivenVectorWithNaNAndExplicitName_WhenRequireNotNaN_ThenThrowsForComponentWithExplicitName()
    {
        // Given
        var v = new Vector2(float.NaN, 0f);

        // When
        Action act = () => Guard.RequireNotNaN(v, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom.X");
    }

    [Fact]
    public void GivenVectorWithInfinityAndNullName_WhenRequireFinite_ThenThrowsForComponentWithValueToken()
    {
        // Given
        var v = new Vector2(1f, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireFinite(v, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>.Y");
    }

    [Fact]
    public void GivenVectorWithInfinityAndNoName_WhenRequireFinite_ThenThrowsForComponentWithCallerName()
    {
        // Given
        var v = new Vector2(1f, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireFinite(v);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("v.Y");
    }

    [Fact]
    public void GivenVectorWithInfinityAndExplicitName_WhenRequireFinite_ThenThrowsForComponentWithExplicitName()
    {
        // Given
        var v = new Vector2(1f, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireFinite(v, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom.Y");
    }

    [Fact]
    public void GivenZeroVectorAndNullName_WhenRequireNonZero_ThenThrowsWithValueToken()
    {
        // Given
        var v = Vector2.Zero;

        // When
        Action act = () => Guard.RequireNonZero(v, null!);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenZeroVectorAndNoName_WhenRequireNonZero_ThenThrowsWithCallerName()
    {
        // Given
        var v = Vector2.Zero;

        // When
        Action act = () => Guard.RequireNonZero(v);

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("v");
    }

    [Fact]
    public void GivenZeroVectorAndExplicitName_WhenRequireNonZero_ThenThrowsWithExplicitName()
    {
        // Given
        var v = Vector2.Zero;

        // When
        Action act = () => Guard.RequireNonZero(v, "custom");

        // Then
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenNonNormalizedVectorAndNullName_WhenRequireNormalized_ThenThrowsWithValueToken()
    {
        // Given
        var v = new Vector2(2f, 0f);

        // When
        Action act = () => Guard.RequireNormalized(v, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>");
    }

    [Fact]
    public void GivenNonNormalizedVectorAndNoName_WhenRequireNormalized_ThenThrowsWithCallerName()
    {
        // Given
        var v = new Vector2(2f, 0f);

        // When
        Action act = () => Guard.RequireNormalized(v);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("v");
    }

    [Fact]
    public void GivenNonNormalizedVectorAndExplicitName_WhenRequireNormalized_ThenThrowsWithExplicitName()
    {
        // Given
        var v = new Vector2(2f, 0f);

        // When
        Action act = () => Guard.RequireNormalized(v, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom");
    }

    [Fact]
    public void GivenInvalidVectorAndNullName_WhenRequireValid_ThenThrowsForComponentWithValueToken()
    {
        // Given
        var v = new Vector2(float.NaN, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireValid(v, null!);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("<value>.X");
    }

    [Fact]
    public void GivenInvalidVectorAndNoName_WhenRequireValid_ThenThrowsForComponentWithCallerName()
    {
        // Given
        var v = new Vector2(float.NaN, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireValid(v);

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("v.X");
    }

    [Fact]
    public void GivenInvalidVectorAndExplicitName_WhenRequireValid_ThenThrowsForComponentWithExplicitName()
    {
        // Given
        var v = new Vector2(float.NaN, float.PositiveInfinity);

        // When
        Action act = () => Guard.RequireValid(v, "custom");

        // Then
        act.Should().Throw<ArgumentException>().WithParameterName("custom.X");
    }
}