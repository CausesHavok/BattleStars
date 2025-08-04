using System.Numerics;
using FluentAssertions;
using Xunit;

namespace BattleStars.Tests;

public class EntityTest
{
    // Minimal concrete subclass for testing purposes
    private class TestEntity(Vector2 position, float health) : Entity(position, health) { }

    private Vector2 _testPosition = new(0, 0);

    [Fact]
    public void GivenNegativeHealth_WhenCreatingEntity_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, -10f);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Health must be positive.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenZeroHealth_WhenCreatingEntity_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, 0f);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Health must be positive.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenEntityWithHealth_WhenTakingDamage_ThenHealthDecreases()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 100f);

        // Act
        entity.TakeDamage(20f);

        // Assert
        entity.Health.Should().Be(80f);
        entity.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenEntityWithLowHealth_WhenTakingExcessiveDamage_ThenHealthIsZeroAndIsDead()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 10f);

        // Act
        entity.TakeDamage(20f);

        // Assert
        entity.Health.Should().Be(0f);
        entity.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenEntityWithHealth_WhenTakingDamageEqualToHealth_ThenHealthIsZeroAndIsDead()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 10f);

        // Act
        entity.TakeDamage(10f);

        // Assert
        entity.Health.Should().Be(0f);
        entity.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenEntityWithPositiveHealth_WhenCheckingIsDead_ThenReturnsFalse()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 50f);

        // Assert
        entity.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenEntityWithHealth_WhenTakingNegativeDamage_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 100f);

        // Act & Assert
        Action act = () => entity.TakeDamage(-10f);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Damage cannot be negative.*")
            .WithParameterName("damage");
    }

    [Fact]
    public void GivenDeadEntity_WhenTakingDamage_ThenHealthDoesNotChange()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 10f);
        entity.TakeDamage(10f);

        // Act
        entity.TakeDamage(20f);

        // Assert
        entity.Health.Should().Be(0f);
        entity.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenEntityWithHealth_WhenTakingZeroDamage_ThenHealthDoesNotChange()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 100f);

        // Act
        entity.TakeDamage(0f);

        // Assert
        entity.Health.Should().Be(100f);
        entity.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenEntityWithHealth_WhenTakingMultipleHits_ThenHealthDecreasesCorrectly()
    {
        // Arrange
        var entity = new TestEntity(_testPosition, 100f);

        // Act
        entity.TakeDamage(20f);
        entity.TakeDamage(30f);
        entity.TakeDamage(10f);

        // Assert
        entity.Health.Should().Be(40f);
        entity.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenEntity_WhenMovingRight_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity right
        entity.Move(new Vector2(5, 0));

        // Assert - Entity has moved to the right
        entity.Position.X.Should().Be(5);
        entity.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenEntity_WhenMovingDown_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity down
        entity.Move(new Vector2(0, 5));

        // Assert - Entity has moved down
        entity.Position.X.Should().Be(0);
        entity.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenEntity_WhenMovingLeft_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity left
        entity.Move(new Vector2(-5, 0));

        // Assert - Entity has moved to the left
        entity.Position.X.Should().Be(-5);
        entity.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenEntity_WhenMovingUp_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity up
        entity.Move(new Vector2(0, -5));

        // Assert - Entity has moved up
        entity.Position.X.Should().Be(0);
        entity.Position.Y.Should().Be(-5);
    }

    [Fact]
    public void GivenEntity_WhenMovingDiagonallyUpRight_ThenMovesDiagonally()
    {
        // Arrange - Entity
        var Entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity up and right
        Entity.Move(new Vector2(5, -5));

        // Assert - Entity has moved diagonally
        Entity.Position.X.Should().Be(5);
        Entity.Position.Y.Should().Be(-5);
    }

    [Fact]
    public void GivenEntity_WhenNoDirectionGiven_ThenPositionDoesNotChange()
    {
        // Arrange - Entity
        var Entity = new TestEntity(_testPosition, 100f);

        // Act - Move Entity with no direction
        Entity.Move(Vector2.Zero);

        // Assert - Entity has not moved
        Entity.Position.X.Should().Be(0);
        Entity.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenEntity_WhenMovingMultipleTimes_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var Entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity right, then up
        Entity.Move(new Vector2(5, 0)); // Move right
        Entity.Move(new Vector2(0, -5)); // Move up

        // Assert - Entity has moved to the new position
        Entity.Position.X.Should().Be(5);
        Entity.Position.Y.Should().Be(-5);
    }

    [Fact]
    public void GivenDeadEntity_WhenMoving_ThenPositionDoesNotChange()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 10f);
        entity.TakeDamage(10f); // Make it dead
        entity.IsDead.Should().BeTrue();

        // Act - try to move dead Entity
        entity.Move(new Vector2(5, 0));

        // Assert - Entity has not moved
        entity.Position.X.Should().Be(0);
        entity.Position.Y.Should().Be(0);
    }

    // Test when health is small fraction that it still behaves correctly
    [Fact]
    public void GivenEntity_WhenHealthIsSmallFraction_ThenDiesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 0.1f);

        // Act 
        entity.TakeDamage(0.1f); // Reduce health to a small fraction

        // Assert - Entity is dead
        entity.IsDead.Should().BeTrue();
    }

    //Test when movement is small fraction that it still behaves correctly
    [Fact]
    public void GivenEntity_WhenMovingSmallFraction_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity by a small fraction
        entity.Move(new Vector2(0.1f, 0.1f));

        // Assert - Entity has moved by the small fraction
        entity.Position.X.Should().Be(0.1f);
        entity.Position.Y.Should().Be(0.1f);
    }

    // Test when damage is float.max value that it still behaves correctly
    [Fact]
    public void GivenEntity_WhenTakingMaxDamage_ThenDiesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, float.MaxValue);

        // Act - take max damage
        entity.TakeDamage(float.MaxValue);

        // Assert - Entity is dead
        entity.IsDead.Should().BeTrue();
    }

    // Test when moving with float.max value that it still behaves correctly
    [Fact]
    public void GivenEntity_WhenMovingWithMaxValue_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity by max value
        entity.Move(new Vector2(float.MaxValue, float.MaxValue));

        // Assert - Entity has moved to the max position
        entity.Position.X.Should().Be(float.MaxValue);
        entity.Position.Y.Should().Be(float.MaxValue);
    }

    // Test for float.nan
    [Fact]
    public void GivenEntity_WhenMovingWithNaN_ThenPositionDoesNotChange()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity by NaN
        Action act = () => entity.Move(new Vector2(float.NaN, float.NaN));

        // Assert - Entity has not moved
        act.Should().Throw<ArgumentException>()
            .WithMessage("Movement vector contains NaN.*")
            .WithParameterName("direction");
    }

    // float.positiveInfinity and float.negativeInfinity
    [Fact]
    public void GivenEntity_WhenMovingWithInfinity_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity with positive and negative infinity
        Action actPositive = () => entity.Move(new Vector2(float.PositiveInfinity, float.PositiveInfinity));
        Action actNegative = () => entity.Move(new Vector2(float.NegativeInfinity, float.NegativeInfinity));

        // Assert - Entity has moved to the infinity position
        actPositive.Should().Throw<ArgumentException>()
            .WithMessage("Movement vector contains Infinity.*")
            .WithParameterName("direction");
        actNegative.Should().Throw<ArgumentException>()
            .WithMessage("Movement vector contains Infinity.*")
            .WithParameterName("direction");
    }

}