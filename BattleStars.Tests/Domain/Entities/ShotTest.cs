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

    [Theory]
    [InlineData(-1f)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.MinValue)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void GivenNegativeOrInvalidSpeed_WhenCreatingShot_ThenThrowsArgumentException(float invalidSpeed)
    {
        var position = new PositionalVector2(0, 0);
        var direction = new DirectionalVector2(1, 0);
        float damage = 10f;

        Action act = () => Shot.Create(position, direction, invalidSpeed, damage);

        act.Should().Throw<ArgumentException>().WithMessage("*speed*");
    }

    [Theory]
    [InlineData(-1f)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.MinValue)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void GivenNegativeOrInvalidDamage_WhenCreatingShot_ThenThrowsArgumentException(float invalidDamage)
    {
        var position = new PositionalVector2(0, 0);
        var direction = new DirectionalVector2(1, 0);
        float speed = 10f;

        Action act = () => Shot.Create(position, direction, speed, invalidDamage);

        act.Should().Throw<ArgumentException>().WithMessage("*damage*");
    }


    [Fact]
    public void GivenValidParameters_WhenShotConstructed_ThenPropertiesAreSetCorrectly()
    {
        var position = new PositionalVector2(1, 2);
        var direction = new DirectionalVector2(0, 1);
        float speed = 5f;
        float damage = 10f;

        var shot = Shot.Create(position, direction, speed, damage);

        shot.Position.Should().Be(position);
        shot.Direction.Should().Be(direction);
        shot.Speed.Should().Be(speed);
        shot.Damage.Should().Be(damage);
        shot.IsActive.Should().BeTrue();
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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);
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
        var shot = Shot.Create(position, direction, speed, damage);
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
        var shot = Shot.Create(position, direction, speed, damage);

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
        var shot = Shot.Create(position, direction, speed, damage);
        shot.Deactivate(); // Deactivate the shot

        // Act
        shot.Deactivate(); // Attempt to deactivate again

        // Assert
        shot.IsActive.Should().BeFalse(); // Should still be inactive
    }

    #endregion
}