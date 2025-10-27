using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class FloatGuardTest
{
    #region NotNaN
    [Fact]
    public void GivenNaN_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireNotNaN(float.NaN, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be NaN.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenNonNaN_WhenRequireNotNaN_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireNotNaN(value, "test");
        act.Should().NotThrow();
    }

    #endregion
    #region Finite
    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInfinity_WhenRequireFinite_ThenThrowsArgumentException(float value)
    {
        Action act = () => FloatGuard.RequireFinite(value, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test must be finite.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenNonInfinity_WhenRequireFinite_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireFinite(value, "test");
        act.Should().NotThrow();
    }

    #endregion
    #region Valid
    [Fact]
    public void GivenInvalid_WhenRequireValid_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireValid(float.NaN, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be NaN.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequireValid(float.PositiveInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test must be finite.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequireValid(float.NegativeInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test must be finite.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenValid_WhenRequireValid_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireValid(value, "test");
        act.Should().NotThrow();
    }
    #endregion

    #region Negative

    [Fact]
    public void GivenNegative_WhenRequireNonNegative_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonNegative(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be negative.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    public void GivenNonNegative_WhenValidated_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireNonNegative(value, "test");
        act.Should().NotThrow();
    }

    #endregion

    #region Zero
     [Fact]
    public void GivenZero_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonZero(0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequireNonZero(float.NegativeZero, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenNonZero_WhenValidatedAgainstZero_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireNonZero(value, "test");
        act.Should().NotThrow();
    }
    #endregion

    #region Positive
    [Fact]
    public void GivenNonPositive_WhenRequirePositive_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequirePositive(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test must be positive.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequirePositive(0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test must be positive.*")
            .WithParameterName("test");
            
        act = () => FloatGuard.RequirePositive(-0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test must be positive.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.MaxValue)]
    public void GivenPositive_WhenRequirePositive_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequirePositive(value, "test");
        act.Should().NotThrow();
    }
    #endregion
}
