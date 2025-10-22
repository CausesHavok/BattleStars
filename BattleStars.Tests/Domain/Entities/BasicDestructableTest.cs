using FluentAssertions;
using BattleStars.Domain.Entities;

namespace BattleStars.Tests.Domain.Entities;

public class BasicDestructableTest
{
    #region Helpers

    #endregion


    #region Constructor Tests
    /*
        - Test that positive health initializes correctly
        - Test that zero or negative health throws an exception
        - Test that Nan or Infinity health throws an exception
    */

    [Fact]
    public void GivenPositiveInputHealth_WhenConstructingBasicDestructable_ThenInitializesCorrectly()
    {
        // Arrange
        float initialHealth = 100f;

        // Act
        var destructable = new BasicDestructable(initialHealth);

        // Assert
        destructable.Health.Should().Be(initialHealth);
        destructable.IsDestroyed.Should().BeFalse();
    }

    [Fact]
    public void GivenNegativeInputHealth_WhenConstructingBasicDestructable_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new BasicDestructable(-50f));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenZeroInputHealth_WhenConstructingBasicDestructable_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new BasicDestructable(0f));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GivenNaNInputHealth_WhenConstructingBasicDestructable_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new BasicDestructable(float.NaN));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void GivenInfinityInputHealth_WhenConstructingBasicDestructable_ThenThrowsArgumentException()
    {
        // Arrange & Act
        Action act = () => new BasicDestructable(float.PositiveInfinity);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GivenNegativeInfinityInputHealth_WhenConstructingBasicDestructable_ThenThrowsArgumentException()
    {
        // Arrange & Act
        var exception = Record.Exception(() => new BasicDestructable(float.NegativeInfinity));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }


    #endregion

    #region TakeDamage Tests
    /*
        - Test that taking damage reduces health correctly
        - Test that taking damage that exceeds current health sets health to zero and marks as destroyed
        - Test that taking negative damage throws an exception
        - Test that taking NaN or Infinity damage throws an exception
        - Test that taking damage on an already destroyed object does not change health
        - Test that taking zero damage does not change health
        - Test multiple damage applications in sequence
    */

    [Fact]
    public void GivenValidDamage_WhenTakingDamage_ThenHealthReducesCorrectly()
    {
        // Arrange
        var destructable = new BasicDestructable(100f);
        float damage = 30f;

        // Act
        destructable.TakeDamage(damage);

        // Assert
        destructable.Health.Should().Be(70f);
        destructable.IsDestroyed.Should().BeFalse();
    }

    [Fact]
    public void GivenExcessiveDamage_WhenTakingDamage_ThenHealthSetsToZeroAndIsDestroyed()
    {
        // Arrange
        var destructable = new BasicDestructable(50f);
        float damage = 100f;

        // Act
        destructable.TakeDamage(damage);

        // Assert
        destructable.Health.Should().Be(0f);
        destructable.IsDestroyed.Should().BeTrue();
    }

    [Fact]
    public void GivenNegativeDamage_WhenTakingDamage_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var destructable = new BasicDestructable(100f);
        float damage = -20f;

        // Act
        var exception = Record.Exception(() => destructable.TakeDamage(damage));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void GivenInvalidDamage_WhenTakingDamage_ThenThrowsArgumentException(float damage)
    {
        // Arrange
        var destructable = new BasicDestructable(100f);

        // Act
        var exception = Record.Exception(() => destructable.TakeDamage(damage));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void GivenDestroyedObject_WhenTakingDamage_ThenHealthRemainsZero()
    {
        // Arrange
        var destructable = new BasicDestructable(50f);
        destructable.TakeDamage(100f); // Destroy the object

        // Act
        destructable.TakeDamage(20f); // Attempt to take more damage

        // Assert
        destructable.Health.Should().Be(0f);
        destructable.IsDestroyed.Should().BeTrue();
    }

    [Fact]
    public void GivenZeroDamage_WhenTakingDamage_ThenHealthRemainsUnchanged()
    {
        // Arrange
        var destructable = new BasicDestructable(100f);
        float damage = 0f;

        // Act
        destructable.TakeDamage(damage);

        // Assert
        destructable.Health.Should().Be(100f);
        destructable.IsDestroyed.Should().BeFalse();
    }

    [Fact]
    public void GivenMultipleDamageApplications_WhenTakingDamage_ThenHealthReducesCorrectly()
    {
        // Arrange
        var destructable = new BasicDestructable(100f);

        // Act
        destructable.TakeDamage(20f);
        destructable.TakeDamage(30f);
        destructable.TakeDamage(10f);

        // Assert
        destructable.Health.Should().Be(40f);
        destructable.IsDestroyed.Should().BeFalse();
    }





    #endregion


}
