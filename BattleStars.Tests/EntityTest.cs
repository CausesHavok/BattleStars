using System.Numerics;
using FluentAssertions;


namespace BattleStars.Tests;

public class EntityTest
{
    #region Supporting Classes
    // Minimal concrete subclass for testing purposes

    
    private class TestEntity : Entity {

        private static readonly Func<Vector2, Vector2, IShot> testShotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);
        public TestEntity(Vector2 position, float health)
            : base(position, health, testShotFactory) { }

    }

    private Vector2 _testPosition = new(0, 0);
    #endregion

    #region Construction Tests
    /* --- Construction logic ---
        * - Entity should be created with valid position and health
        * - Should throw exceptions for invalid health values (negative, zero, NaN, Infinity)
        * - Should throw exceptions for invalid position values (NaN, Infinity)
    */

    [Fact]
    public void GivenValidHealthAndPosition_WhenCreatingEntity_ThenEntityIsCreated()
    {
        // Arrange & Act
        var entity = new TestEntity(_testPosition, 100f);

        // Assert
        entity.Should().NotBeNull();
        entity.Position.Should().Be(_testPosition);
        entity.Health.Should().Be(100f);
    }

    [Fact]
    public void GivenNegativeHealth_WhenCreatingEntity_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, -10f);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("health cannot be negative.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenZeroHealth_WhenCreatingEntity_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, 0f);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("health cannot be zero.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenNaNHealth_WhenCreatingEntity_ThenThrowsArgumentException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, float.NaN);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("health cannot be NaN.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenInfinityHealth_WhenCreatingEntity_ThenThrowsArgumentException()
    {
        // Arrange & Act
        Action act = () => new TestEntity(_testPosition, float.PositiveInfinity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("health cannot be Infinity.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenNaNPosition_WhenCreatingEntity_ThenThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(float.NaN, 0);

        // Act
        Action act = () => new TestEntity(position, 100f);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("position.X cannot be NaN.*")
            .WithParameterName("position.X");
    }

    [Fact]
    public void GivenInfinityPosition_WhenCreatingEntity_ThenThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(float.PositiveInfinity, 0);

        // Act
        Action act = () => new TestEntity(position, 100f);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("position.X cannot be Infinity.*")
            .WithParameterName("position.X");
    }


    #endregion

    #region Damage/Health Tests
    /* --- Damage/health logic ---
        * - Entity should be able to take damage
        * - Health should decrease by the amount of damage taken (unless see edge cases)
        * - Should handle cases where health to zero (entity should be marked as dead)
        * - Should handle edge cases (e.g., taking more damage than current health)
        * - Should throw exceptions for invalid damage values (negative, NaN, Infinity)
        * - Should not allow damage to change health if entity is already dead
    */

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
    public void GivenEntity_WhenHealthIsSmallFraction_ThenDiesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 0.1f);

        // Act 
        entity.TakeDamage(0.1f); // Reduce health to a small fraction

        // Assert - Entity is dead
        entity.IsDead.Should().BeTrue();
    }

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

    [Fact]
    public void GivenEntity_WhenTakingNaNDamage_ThenThrowsArgumentException()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - take NaN damage
        Action act = () => entity.TakeDamage(float.NaN);

        // Assert - Entity has not moved
        act.Should().Throw<ArgumentException>()
            .WithMessage("damage cannot be NaN.*")
            .WithParameterName("damage");
    }

    [Fact]
    public void GivenEntity_WhenTakingInfinityDamage_ThenThrowsArgumentException()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - take Infinity damage
        Action act = () => entity.TakeDamage(float.PositiveInfinity);

        // Assert - Entity has not moved
        act.Should().Throw<ArgumentException>()
            .WithMessage("damage cannot be Infinity.*")
            .WithParameterName("damage");
    }

    #endregion

    #region Movement Tests
    /* --- Movement logic ---
        * - Entity should be able to move in all directions (up, down, left, right)
        * - Position should update correctly based on movement direction
        * - Should handle diagonal movement
        * - Should not allow movement if entity is dead
        * - Should handle cases where no direction is given (position should not change)
        * - Should handle small fractions and large values correctly
        * - Should throw exceptions for invalid movement values (NaN, Infinity)
    */

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
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity up and right
        entity.Move(new Vector2(5, -5));

        // Assert - Entity has moved diagonally
        entity.Position.X.Should().Be(5);
        entity.Position.Y.Should().Be(-5);
    }

    [Fact]
    public void GivenEntity_WhenNoDirectionGiven_ThenPositionDoesNotChange()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - Move Entity with no direction
        entity.Move(Vector2.Zero);

        // Assert - Entity has not moved
        entity.Position.X.Should().Be(0);
        entity.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenEntity_WhenMovingMultipleTimes_ThenPositionUpdatesCorrectly()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity right, then up
        entity.Move(new Vector2(5, 0)); // Move right
        entity.Move(new Vector2(0, -5)); // Move up

        // Assert - Entity has moved to the new position
        entity.Position.X.Should().Be(5);
        entity.Position.Y.Should().Be(-5);
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

    [Fact]
    public void GivenEntity_WhenMovingWithNaN_ThenThrowsArgumentException()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity by NaN
        Action act = () => entity.Move(new Vector2(float.NaN, float.NaN));

        // Assert - Entity has not moved
        act.Should().Throw<ArgumentException>()
            .WithMessage("direction.X cannot be NaN.*")
            .WithParameterName("direction.X");
    }

    [Fact]
    public void GivenEntity_WhenMovingWithInfinity_ThenThrowsArgumentException()
    {
        // Arrange - Entity
        var entity = new TestEntity(_testPosition, 100f);

        // Act - move Entity with positive and negative infinity
        Action actPositive = () => entity.Move(new Vector2(float.PositiveInfinity, float.PositiveInfinity));
        Action actNegative = () => entity.Move(new Vector2(float.NegativeInfinity, float.NegativeInfinity));

        // Assert - Entity has moved to the infinity position
        actPositive.Should().Throw<ArgumentException>()
            .WithMessage("direction.X cannot be Infinity.*")
            .WithParameterName("direction.X");
        actNegative.Should().Throw<ArgumentException>()
            .WithMessage("direction.X cannot be Infinity.*")
            .WithParameterName("direction.X");
    }

    #endregion

}