using FluentAssertions;
using BattleStars.Core.Guards;

namespace BattleStars.Tests.Core.Guards;

public class NullGuardTest
{
    [Fact]
    public void GivenNullValue_WhenNotNullIsCalled_ThenThrowsArgumentNullException()
    {
        // Given
        object? sut = null;

        // When
        Action Act = () => Guard.NotNull(sut, nameof(sut));

        // Then
        Act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNonNullValue_WhenNotNullIsCalled_ThenReturnsSameInstance()
    {
        // Given
        var sut = new object();

        // When
        var result = Guard.NotNull(sut, nameof(sut));

        // Then
        result.Should().BeSameAs(sut);
    }
}