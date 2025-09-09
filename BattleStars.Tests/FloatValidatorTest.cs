using FluentAssertions;
using BattleStars.Utility;

namespace BattleStars.Tests;

public class FloatValidatorTest
{
    [Fact]
    public void GivenNaN_WhenValidated_ThenThrowsArgumentException()
    {
        Action act = () => FloatValidator.ThrowIfNaN(float.NaN, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be NaN.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenInfinity_WhenValidated_ThenThrowsArgumentException()
    {
        Action act = () => FloatValidator.ThrowIfInfinity(float.PositiveInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenNegativeInfinity_WhenValidated_ThenThrowsArgumentException()
    {
        Action act = () => FloatValidator.ThrowIfInfinity(float.NegativeInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenNegative_WhenValidated_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatValidator.ThrowIfNegative(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be negative.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenZero_WhenValidated_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatValidator.ThrowIfZero(0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenNaNOrInfinity_WhenValidated_ThenThrowsArgumentException()
    {
        Action act = () => FloatValidator.ThrowIfNaNOrInfinity(float.NaN, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be NaN.*")
            .WithParameterName("test");

        act = () => FloatValidator.ThrowIfNaNOrInfinity(float.PositiveInfinity, "test");
        act.Should().Throw<ArgumentException>()
            .WithMessage("test cannot be Infinity.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenValidFloat_WhenValidated_DoesNotThrow()
    {
        Action act = () => FloatValidator.ThrowIfNaNOrInfinity(1.0f, "test");
        act.Should().NotThrow();

        act = () => FloatValidator.ThrowIfNegative(1.0f, "test");
        act.Should().NotThrow();

        act = () => FloatValidator.ThrowIfZero(1.0f, "test");
        act.Should().NotThrow();
    }

    [Fact]
    public void GivenValidMaxFloat_WhenValidated_DoesNotThrow()
    {
        Action act = () => FloatValidator.ThrowIfNaNOrInfinity(float.MaxValue, "test");
        act.Should().NotThrow();

        act = () => FloatValidator.ThrowIfNegative(float.MaxValue, "test");
        act.Should().NotThrow();

        act = () => FloatValidator.ThrowIfZero(float.MaxValue, "test");
        act.Should().NotThrow();
    }

    [Fact]
    public void GivenValidMinFloat_WhenValidated_DoesNotThrow()
    {
        Action act = () => FloatValidator.ThrowIfNaNOrInfinity(float.MinValue, "test");
        act.Should().NotThrow();

        act = () => FloatValidator.ThrowIfZero(float.MinValue, "test");
        act.Should().NotThrow();
    }

    [Fact]
    public void GivenNegativeZero_WhenValidatedAgainstZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatValidator.ThrowIfZero(-0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenNegative_WhenValidatedAgainstNegativeOrZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatValidator.ThrowIfNegativeOrZero(-1f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be negative.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenZero_WhenValidatedAgainstNegativeOrZero_ThenThrowsArgumentOutOfRangeException()
    {
        Action act = () => FloatValidator.ThrowIfNegativeOrZero(0f, "test");
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("test cannot be zero.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenPositive_WhenValidatedAgainstNegativeOrZero_ThenDoesNotThrow()
    {
        Action act = () => FloatValidator.ThrowIfNegativeOrZero(1f, "test");
        act.Should().NotThrow();
    }
}
