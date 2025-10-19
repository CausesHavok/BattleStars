using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class FloatGuardTest
{
    #region NaN

    [Fact]
    public void GivenNullName_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireNotNaN(float.NaN, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*<value> cannot be NaN.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireNotNaN_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireNotNaN(float.NaN);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*float.NaN cannot be NaN.*")
            .WithParameterName("float.NaN");
    }

    [Fact]
    public void GivenName_WhenRequireNotNaN_ThenThrowsArgumentException()
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
        Action act = () => FloatGuard.RequireNotNaN(value);
        act.Should().NotThrow();
    }

    #endregion

    #region Finite

    [Fact]
    public void GivenNullName_WhenRequireFinite_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireFinite(float.PositiveInfinity, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("<value> must be finite.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireFinite_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireFinite(float.PositiveInfinity);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*float.PositiveInfinity must be finite.*")
            .WithParameterName("float.PositiveInfinity");
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void GivenName_WhenRequireFinite_ThenThrowsArgumentException(float value)
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
        Action act = () => FloatGuard.RequireFinite(value);
        act.Should().NotThrow();
    }

    #endregion

    #region RequireValid

    [Fact]
    public void GivenNullName_WhenRequireValid_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireValid(float.NaN, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*<value> cannot be NaN.*")
            .WithParameterName("<value>");
        
        act = () => FloatGuard.RequireValid(float.PositiveInfinity, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*<value> must be finite.*")
            .WithParameterName("<value>");

        act = () => FloatGuard.RequireValid(float.NegativeInfinity, null!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*<value> must be finite.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireValid_ThenThrowsArgumentException()
    {
        Action act = () => FloatGuard.RequireValid(float.NaN);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*float.NaN cannot be NaN.*")
            .WithParameterName("float.NaN");

        act = () => FloatGuard.RequireValid(float.PositiveInfinity);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*float.PositiveInfinity must be finite.*")
            .WithParameterName("float.PositiveInfinity");
        
        act = () => FloatGuard.RequireValid(float.NegativeInfinity);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*float.NegativeInfinity must be finite.*")
            .WithParameterName("float.NegativeInfinity");
    }

    [Fact]
    public void GivenName_WhenRequireValid_ThenThrowsArgumentException()
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
        Action act = () => FloatGuard.RequireValid(value);
        act.Should().NotThrow();
    }
    #endregion

    #region Negative

    [Fact]
    public void GivenNullName_WhenRequireNonNegative_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonNegative(-1f, null!);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*<value> cannot be negative.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireNonNegative_ThenDoesNotThrow()
    {
        Action act = () => FloatGuard.RequireNonNegative(-1f);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*-1f cannot be negative.*")
            .WithParameterName("-1f");
    }

    [Fact]
    public void GivenName_WhenRequireNonNegative_ThenThrowsArgumentOutOfRangeException()
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
        Action act = () => FloatGuard.RequireNonNegative(value);
        act.Should().NotThrow();
    }

    #endregion

    #region Zero
    [Fact]
    public void GivenNullName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonZero(0f, null!);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*<value> cannot be zero.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequireNonZero(0f);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*0f cannot be zero.*")
            .WithParameterName("0f");
    }

    [Fact]
    public void GivenName_WhenRequireNonZero_ThenThrowsArgumentOutOfRangeException()
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
        Action act = () => FloatGuard.RequireNonZero(value);
        act.Should().NotThrow();
    }
    #endregion

    #region Positive

    [Fact]
    public void GivenNullName_WhenRequirePositive_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequirePositive(0f, null!);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*<value> must be positive.*")
            .WithParameterName("<value>");
    }

    [Fact]
    public void GivenNoName_WhenRequirePositive_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatGuard.RequirePositive(0f);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*0f must be positive.*")
            .WithParameterName("0f");
    }

    [Fact]
    public void GivenName_WhenRequirePositive_ThenThrowsArgumentOutOfRangeException()
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
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.MaxValue)]
    public void GivenPositive_WhenRequirePositive_ThenDoesNotThrow(float value)
    {
        Action act = () => FloatGuard.RequirePositive(value);
        act.Should().NotThrow();
    }
    #endregion
}
