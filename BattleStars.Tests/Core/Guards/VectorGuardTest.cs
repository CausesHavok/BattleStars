using System.Numerics;
using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class VectorGuardTest
{
    [Fact]
    public void GivenNaN_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, 1f);
        Action act = () => VectorGuard.RequireNotNaN(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }

    [Fact]
    public void GivenInfinity_WhenRequireFinite_ThenThrowsArgumentException()
    {
        var vector = new Vector2(1f, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireFinite(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.Y must be finite.*")
            .WithParameterName("testVector.Y");
    }

    [Fact]
    public void GivenZero_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        var vector = Vector2.Zero;
        Action act = () => VectorGuard.RequireNonZero(vector, "testVector");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("testVector cannot be zero vector.*")
            .WithParameterName("testVector");
    }

    [Fact]
    public void GivenUnNormalized_WhenRequireNormalized_ThenThrowsArgumentException()
    {
        var vector = new Vector2(2, 0);
        Action act = () => VectorGuard.RequireNormalized(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector must be a normalized vector.*")
            .WithParameterName("testVector");
    }

    [Fact]
    public void GivenInvalid_WhenRequireValid_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireValid(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }

    [Fact]
    public void GivenValidVector_WhenAllValidatorsCalled_ThenDoesNotThrow()
    {
        var vector = Vector2.Normalize(new Vector2(3, 4));
        Action act = () =>
        {
            VectorGuard.RequireNotNaN(vector, "testVector");
            VectorGuard.RequireFinite(vector, "testVector");
            VectorGuard.RequireNormalized(vector, "testVector");
            VectorGuard.RequireValid(vector, "testVector");
            VectorGuard.RequireNonZero(vector, "testVector");
        };

        act.Should().NotThrow();
    }
}