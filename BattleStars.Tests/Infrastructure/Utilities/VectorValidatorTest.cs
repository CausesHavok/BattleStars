using System.Numerics;
using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Infrastructure.Utilities;

public class VectorValidatorTest
{
    [Fact]
    public void GivenVectorWithNaNComponent_WhenThrowIfNaN_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, 1f);
        Action act = () => VectorGuard.ThrowIfNaN(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }

    [Fact]
    public void GivenVectorWithInfinityComponent_WhenThrowIfInfinity_ThenThrowsArgumentException()
    {
        var vector = new Vector2(1f, float.PositiveInfinity);
        Action act = () => VectorGuard.ThrowIfInfinity(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.Y cannot be Infinity.*")
            .WithParameterName("testVector.Y");
    }

    [Fact]
    public void GivenZeroVector_WhenThrowIfZero_ThenThrowsArgumentOutOfRangeException()
    {
        var vector = Vector2.Zero;
        Action act = () => VectorGuard.ThrowIfZero(vector, "testVector");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("testVector cannot be a zero vector.*")
            .WithParameterName("testVector");
    }

    [Fact]
    public void GivenNonNormalizedVector_WhenThrowIfNotNormalized_ThenThrowsArgumentException()
    {
        var vector = new Vector2(2, 0);
        Action act = () => VectorGuard.ThrowIfNotNormalized(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector must be a normalized vector.*")
            .WithParameterName("testVector");
    }

    [Fact]
    public void GivenNormalizedVector_WhenThrowIfNotNormalized_ThenDoesNotThrow()
    {
        var vector = Vector2.Normalize(new Vector2(1, 1));
        vector = Vector2.Normalize(vector); // Ensure it's normalized
        Action act = () => VectorGuard.ThrowIfNotNormalized(vector, "testVector");
        act.Should().NotThrow();
    }

    [Fact]
    public void GivenVectorWithNaNOrInfinity_WhenThrowIfNaNOrInfinity_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, float.PositiveInfinity);
        Action act = () => VectorGuard.ThrowIfNaNOrInfinity(vector, "testVector");
        act.Should().Throw<ArgumentException>("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }

    [Fact]
    public void GivenValidVector_WhenAllValidatorsCalled_ThenDoesNotThrow()
    {
        var vector = Vector2.Normalize(new Vector2(3, 4));
        Action act = () =>
        {
            VectorGuard.ThrowIfNaN(vector, "testVector");
            VectorGuard.ThrowIfInfinity(vector, "testVector");
            VectorGuard.ThrowIfNotNormalized(vector, "testVector");
            VectorGuard.ThrowIfNaNOrInfinity(vector, "testVector");
            VectorGuard.ThrowIfZero(vector, "testVector");
        };

        act.Should().NotThrow();
    }
}