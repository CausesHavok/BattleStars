using System.Numerics;
using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class VectorGuardTest
{
    #region NaN
    [Fact]
    public void GivenNullName_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, 1f);
        Action act = () => VectorGuard.RequireNotNaN(vector, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("<value>.X cannot be NaN.*")
            .WithParameterName("<value>.X");
    }

    [Fact]
    public void GivenNoName_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, 1f);
        Action act = () => VectorGuard.RequireNotNaN(vector);
        act.Should().Throw<ArgumentException>()
            .WithMessage("vector.X cannot be NaN.*")
            .WithParameterName("vector.X");
    }

    [Fact]
    public void GivenName_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, 1f);
        Action act = () => VectorGuard.RequireNotNaN(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }

    #endregion

    #region Finite
    [Fact]
    public void GivenNullName_WhenRequireFinite_ThenThrowsArgumentException()
    {
        var vector = new Vector2(1f, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireFinite(vector, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("<value>.Y must be finite.*")
            .WithParameterName("<value>.Y");
    }

    [Fact]
    public void GivenNoName_WhenRequireFinite_ThenThrowsArgumentException()
    {
        var vector = new Vector2(1f, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireFinite(vector);
        act.Should().Throw<ArgumentException>()
            .WithMessage("vector.Y must be finite.*")
            .WithParameterName("vector.Y");
    }

    [Fact]
    public void GivenName_WhenRequireFinite_ThenThrowsArgumentException()
    {
        var vector = new Vector2(1f, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireFinite(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.Y must be finite.*")
            .WithParameterName("testVector.Y");
    }

    #endregion

    #region NonZero
    [Fact]
    public void GivenNullName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        var vector = Vector2.Zero;
        Action act = () => VectorGuard.RequireNonZero(vector, null!);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("<value> cannot be a zero vector.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        var vector = Vector2.Zero;
        Action act = () => VectorGuard.RequireNonZero(vector);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("vector cannot be a zero vector.*")
            .WithParameterName("vector");
    }

    [Fact]
    public void GivenName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        var vector = Vector2.Zero;
        Action act = () => VectorGuard.RequireNonZero(vector, "testVector");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("testVector cannot be a zero vector.*")
            .WithParameterName("testVector");
    }

    #endregion

    #region Normalized

    [Fact]
    public void GivenNullName_WhenRequireNormalized_ThenThrowsArgumentException()
    {
        var vector = new Vector2(2, 0);
        Action act = () => VectorGuard.RequireNormalized(vector, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("<value> must be a normalized vector.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireNormalized_ThenThrowsArgumentException()
    {
        var vector = new Vector2(2, 0);
        Action act = () => VectorGuard.RequireNormalized(vector);
        act.Should().Throw<ArgumentException>()
            .WithMessage("vector must be a normalized vector.*")
            .WithParameterName("vector");
    }

    [Fact]
    public void GivenName_WhenRequireNormalized_ThenThrowsArgumentException()
    {
        var vector = new Vector2(2, 0);
        Action act = () => VectorGuard.RequireNormalized(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector must be a normalized vector.*")
            .WithParameterName("testVector");
    }

    #endregion

    #region Valid

    [Fact]
    public void GivenNullName_WhenRequireValid_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireValid(vector, null!);
        act.Should().Throw<ArgumentException>("<value>.X cannot be NaN.*")
            .WithParameterName("<value>.X");
    }

    [Fact]
    public void GivenNoName_WhenRequireValid_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireValid(vector);
        act.Should().Throw<ArgumentException>()
            .WithMessage("vector.X cannot be NaN.*")
            .WithParameterName("vector.X");
    }

    [Fact]
    public void GivenName_WhenRequireValid_ThenThrowsArgumentException()
    {
        var vector = new Vector2(float.NaN, float.PositiveInfinity);
        Action act = () => VectorGuard.RequireValid(vector, "testVector");
        act.Should().Throw<ArgumentException>()
            .WithMessage("testVector.X cannot be NaN.*")
            .WithParameterName("testVector.X");
    }
    #endregion

    #region All Validators

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
    #endregion
}