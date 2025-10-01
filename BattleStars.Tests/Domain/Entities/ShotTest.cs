using System.Numerics;
using FluentAssertions;
using BattleStars.Domain.Entities;
using BattleStars.Domain.ValueObjects;

namespace BattleStars.Tests.Entities;

public class ShotTest
{
    /* --- Tests Shot class functionality
    */

    #region Constructor Tests
    /*
        * Tests the construction of properties
        * Tests the damage
            * NaN or Infinity values are not allowed
            * Negative values are not allowed
            * Other values are allowed
        * Tests the Speed property
            * NaN or Infinity values are not allowed
            * Negative values are not allowed
            * Other values are allowed
        * Tests the IsActive property
            * Should be true by default
        * NOTE. PositionalVector2 and DirectionalVector2 are tested in their respective test classes
    */

    [Fact]
    public void GivenShot_WhenConstructed_PropertiesAreSetCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        var speed = 5f;
        var damage = 10f;

        // Act
        var shot = new Shot(position, direction, speed, damage);

        // Assert
        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(speed);
        shot.Damage.Should().Be(damage);
        shot.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenShot_WhenSpeedIsNaN_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = float.NaN;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("speed cannot be NaN.*");
    }

    [Fact]
    public void GivenShot_WhenSpeedIsInfinity_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = float.PositiveInfinity;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("speed cannot be Infinity.*");
    }

    [Fact]
    public void GivenShot_WhenSpeedIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = -5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("speed cannot be negative.*");
    }

    [Fact]
    public void GivenShot_WhenDamageIsNaN_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = float.NaN;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("damage cannot be NaN*");
    }

    [Fact]
    public void GivenShot_WhenDamageIsInfinity_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = float.PositiveInfinity;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("damage cannot be Infinity.*");
    }

    [Fact]
    public void GivenShot_WhenDamageIsNegative_ThrowsArgumentException()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = -10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("damage cannot be negative.*");
    }

    #endregion

    #region Update Tests
    /* Tests the Update method
        * If the shot is deactivated, it should not update its position
        * If the speed is zero, it should not update its position
        * Should update the position based on the direction and speed
            * Up
            * Down
            * Left
            * Right
            * Diagonal movements
    */

    [Fact]
    public void GivenShot_WhenUpdateCalled_ThenUpdatesPositionBasedOnDirectionAndSpeed()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitX); // Right
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new PositionalVector2(6, 2)); // Position should be updated by speed in the direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithLeftDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(5, 5);
        var direction = new DirectionalVector2(-Vector2.UnitX); // Left
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new PositionalVector2(2, 5)); // Position should be updated by speed in the left direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithRightDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(5, 5);
        var direction = new DirectionalVector2(Vector2.UnitX); // Right
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new PositionalVector2(8, 5)); // Position should be updated by speed in the right direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithDownDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(5, 5);
        var direction = new DirectionalVector2(-Vector2.UnitY); // Down
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new PositionalVector2(5, 2)); // Position should be updated by speed in the down direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithUpDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(5, 5);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new PositionalVector2(5, 8));
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithDiagonalMovement_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new PositionalVector2(5, 5);
        var direction = new DirectionalVector2(Vector2.Normalize(new Vector2(1, -1))); // Diagonal (Down-Right)
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.X.Should().BeApproximately(7.12f, 0.01f); // Position should be updated by speed in the diagonal direction
        shot.Position.Y.Should().BeApproximately(2.88f, 0.01f); // Position should be updated by speed in the diagonal direction
    }


    [Fact]
    public void GivenShot_WhenUpdateCalled_WithZeroSpeed_ThenDoesNotUpdatePosition()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitX); // Right
        float speed = 0f; // No speed
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(position); // Position should remain unchanged
    }

    [Fact]
    public void GivenDeadShot_WhenUpdateCalled_ThenDoesNotUpdatePosition()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitX); // Right
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);
        shot.Deactivate(); // Simulate the shot being dead

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(position); // Position should remain unchanged
    }

    [Fact]
    public void GivenShot_WhenDeactivateCalled_ThenDoesNotUpdatePosition()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);
        shot.Deactivate(); // Deactivate the shot

        // Act
        shot.Update(); // Attempt to update after deactivation

        // Assert
        shot.Position.Should().Be(position); // Position should remain unchanged
    }

    #endregion

    #region Deactivate Tests
    /* Tests the Deactivate method
        * Should set IsActive to false
        * Should do nothing if already inactive
    */

    [Fact]
    public void GivenShot_WhenDeactivateCalled_ThenSetsIsActiveToFalse()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Deactivate();

        // Assert
        shot.IsActive.Should().BeFalse();
    }

    [Fact]
    public void GivenInactiveShot_WhenDeactivateCalled_ThenDoesNothing()
    {
        // Arrange
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(Vector2.UnitY); // Up
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);
        shot.Deactivate(); // Deactivate the shot

        // Act
        shot.Deactivate(); // Attempt to deactivate again

        // Assert
        shot.IsActive.Should().BeFalse(); // Should still be inactive
    }

    #endregion

}
