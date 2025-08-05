using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests;

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
        * Tests the position
            * NaN or Infinity values are not allowed
            * Other values are allowed
        * Tests the direction
            * NaN or Infinity values are not allowed
            * Values must be normalized
            * Other values are allowed
        * Tests the Speed property
            * NaN or Infinity values are not allowed
            * Negative values are not allowed
            * Other values are allowed
        * Tests the IsActive property
            * Should be true by default
    */

    [Fact]
    public void GivenShot_WhenConstructed_PropertiesAreSetCorrectly()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
    public void GivenShot_WhenPositionIsNaN_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(float.NaN, 2);
        var direction = new Vector2(0, 1); // Up
        float speed = 5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("Position.X cannot be NaN.*");
    }

    [Fact]
    public void GivenShot_WhenPositionIsInfinity_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(float.PositiveInfinity, 2);
        var direction = new Vector2(0, 1); // Up
        float speed = 5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("position.X cannot be Infinity.*");
    }

    [Fact]
    public void GivenShot_WhenDirectionIsNaN_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var direction = new Vector2(float.NaN, 1); // NaN in X
        float speed = 5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("direction.X cannot be NaN.*");
    }

    [Fact]
    public void GivenShot_WhenDirectionIsInfinity_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var direction = new Vector2(float.PositiveInfinity, 1); // Infinity in X
        float speed = 5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("direction.X cannot be Infinity.*");
    }

    [Fact]
    public void GivenShot_WhenDirectionIsNotNormalized_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var direction = new Vector2(1, 1); // Not normalized
        float speed = 5f;
        float damage = 10f;

        // Act & Assert
        Action act = () => new Shot(position, direction, speed, damage);
        act.Should().Throw<ArgumentException>().WithMessage("direction must be a normalized vector.*");
    }

    [Fact]
    public void GivenShot_WhenSpeedIsNaN_ThrowsArgumentException()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(1, 0); // Right
        float speed = 5f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new Vector2(6, 2)); // Position should be updated by speed in the direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithLeftDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new Vector2(5, 5);
        var direction = new Vector2(-1, 0); // Left
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new Vector2(2, 5)); // Position should be updated by speed in the left direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithRightDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new Vector2(5, 5);
        var direction = new Vector2(1, 0); // Right
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new Vector2(8, 5)); // Position should be updated by speed in the right direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithDownDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new Vector2(5, 5);
        var direction = new Vector2(0, -1); // Down
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new Vector2(5, 2)); // Position should be updated by speed in the down direction
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithUpDirection_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new Vector2(5, 5);
        var direction = new Vector2(0, 1); // Up
        float speed = 3f;
        float damage = 10f;
        var shot = new Shot(position, direction, speed, damage);

        // Act
        shot.Update();

        // Assert
        shot.Position.Should().Be(new Vector2(5, 8));
    }

    [Fact]
    public void GivenShot_WhenUpdateCalled_WithDiagonalMovement_ThenUpdatesPositionCorrectly()
    {
        // Arrange
        var position = new Vector2(5, 5);
        var direction = new Vector2(1 / (float)Math.Sqrt(2), -1 / (float)Math.Sqrt(2)); // Diagonal (Down-Right)
        direction = Vector2.Normalize(direction); // Ensure it's normalized
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(1, 0); // Right
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(1, 0); // Right
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
        var position = new Vector2(1, 2);
        var direction = new Vector2(0, 1); // Up
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
