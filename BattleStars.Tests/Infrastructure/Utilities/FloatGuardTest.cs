using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Infrastructure.Utilities;

public class FloatGuardTest
{
    #region NaN

    [Fact]
    public void GivenNullName_WhenValidatedWithNaN_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequireNotNaN(0f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Fact]
    public void GivenNaN_WhenValidated_ThenThrowsArgumentException()
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
    public void GivenNonNaN_WhenValidated_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireNotNaN(value, "test");
        act.Should().NotThrow();
    }

    #endregion

    #region Infinity

    [Fact]
    public void GivenNullName_WhenValidatedWithInfinity_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequireFinite(0f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenInfinity_WhenValidated_ThenThrowsArgumentException(float value)
    {
        Action act = () => FloatGuard.RequireFinite(value, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenNonInfinity_WhenValidated_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireFinite(value, "test");
        act.Should().NotThrow();
    }

    #endregion

    #region NaN or Infinity

    [Fact]
    public void GivenNullName_WhenValidatedWithNaNOrInfinity_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequireValid(0f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Fact]
    public void GivenNaNOrInfinity_WhenValidated_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireValid(float.NaN, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be NaN.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequireValid(float.PositiveInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequireValid(float.NegativeInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(-1f)]
    public void GivenNonNaNOrInfinity_WhenValidated_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequireValid(value, "test");
        act.Should().NotThrow();
    }
    #endregion

    #region Negative

    [Fact]
    public void GivenNullName_WhenValidatedWithNegative_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequireNonNegative(-1f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Fact]
    public void GivenNegative_WhenValidated_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonNegative(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be negative.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
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
    public void GivenNullName_WhenValidatedWithZero_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequireNonZero(0f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Fact]
    public void GivenZero_WhenValidated_ThenThrowsArgumentOutOfRangeException()
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
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
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

    #region Negative or Zero tests

    [Fact]
    public void GivenNullName_WhenValidatedWithNegativeOrZero_ThenThrowsArgumentNullException()
    {
        Action act = () => FloatGuard.RequirePositive(0f, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*");
    }

    [Fact]
    public void GivenNegative_WhenValidatedAgainstNegativeOrZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequirePositive(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be negative.*")
            .WithParameterName("test");

        act = () => FloatGuard.RequirePositive(0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
            
        act = () => FloatGuard.RequirePositive(-0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.MaxValue)]
    public void GivenNonNegativeNonZero_WhenValidatedAgainstNegativeOrZero_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequirePositive(value, "test");
        act.Should().NotThrow();
    }
    #endregion
}
