using FluentAssertions;
using BattleStars.Infrastructure.Utilities;

namespace BattleStars.Tests.Infrastructure.Utilities;

public class GuardTest
{
    #region NotNull

    [Fact]
    public void GivenNullValue_WhenValidatedWithNotNull_ThenThrowsArgumentNullException()
    {
        Action act = () => Guard.NotNull<object>(null!, "test");
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Value cannot be null.*")
            .WithParameterName("test");
    }

    [Fact]
    public void GivenNonNullValue_WhenValidatedWithNotNull_ThenDoesNotThrow()
    {
        Action act = () => Guard.NotNull(new object(), "test");
        act.Should().NotThrow();
    }

    #endregion
}