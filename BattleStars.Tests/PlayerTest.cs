using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests;

public class PlayerTest
{

    private enum StartPosition
    {
        LeftEdge,
        RightEdge,
        TopEdge,
        BottomEdge,
        Center,
        TopLeftCorner,
        TopRightCorner,
        BottomLeftCorner,
        BottomRightCorner
    }

    private static readonly Dictionary<StartPosition, Vector2> StartPositions = new()
    {
        { StartPosition.LeftEdge, new Vector2(0, 5) },
        { StartPosition.RightEdge, new Vector2(10, 5) },
        { StartPosition.TopEdge, new Vector2(5, 0) },
        { StartPosition.BottomEdge, new Vector2(5, 10) },
        { StartPosition.Center, new Vector2(5, 5) },
        { StartPosition.TopLeftCorner, new Vector2(0, 0) },
        { StartPosition.TopRightCorner, new Vector2(10, 0) },
        { StartPosition.BottomLeftCorner, new Vector2(0, 10) },
        { StartPosition.BottomRightCorner, new Vector2(10, 10) }
    };

    private enum MoveDirection
    {
        None,
        Left,
        Right,
        Up,
        Down,
        LeftUp,
        LeftDown,
        RightUp,
        RightDown
    }

    private static readonly Dictionary<MoveDirection, Vector2> MoveVectors = new()
    {
        { MoveDirection.None,     new Vector2(0, 0) },
        { MoveDirection.Left,     new Vector2(-5, 0) },
        { MoveDirection.Right,    new Vector2(5, 0) },
        { MoveDirection.Up,       new Vector2(0, -5) },
        { MoveDirection.Down,     new Vector2(0, 5) },
        { MoveDirection.LeftUp,   new Vector2(-5, -5) },
        { MoveDirection.LeftDown, new Vector2(-5, 5) },
        { MoveDirection.RightUp,  new Vector2(5, -5) },
        { MoveDirection.RightDown,new Vector2(5, 5) }
    };


    private class FakeBoundaryChecker : IBoundaryChecker
    {
        public bool IsOutsideXBounds(float x)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return x < 0 || x > 10;
        }
        public bool IsOutsideYBounds(float y)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return y < 0 || y > 10;
        }
    }

    private class NullBoundaryChecker : IBoundaryChecker
    {
        public bool IsOutsideXBounds(float x) => false;
        public bool IsOutsideYBounds(float y) => false;
    }


    [Fact]
    public void GivenNegativeHealth_WhenCreatingPlayer_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new Player(StartPositions[StartPosition.Center], -10f, new NullBoundaryChecker());

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Health must be positive.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenZeroHealth_WhenCreatingPlayer_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act
        Action act = () => new Player(StartPositions[StartPosition.Center], 0f, new NullBoundaryChecker());

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Health must be positive.*")
            .WithParameterName("health");
    }

    [Fact]
    public void GivenNullBoundaryChecker_WhenCreatingPlayer_ThenThrowsArgumentNullException()
    {
        // Arrange & Act
        #pragma warning disable CS8625
        Action act = () => new Player(StartPositions[StartPosition.Center], 100f, null);
        #pragma warning restore CS8625

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Boundary checker cannot be null.*")
            .WithParameterName("boundaryChecker");
    }

    [Fact]
    public void GivenPlayerAtLeftEdge_WhenMovingLeft_ThenPositionDoesNotChange()
    {
        //Arrange - player at left edge
        var player = new Player(StartPositions[StartPosition.LeftEdge], 100f, new FakeBoundaryChecker());

        //Act - move player left
        player.Move(MoveVectors[MoveDirection.Left]);

        //Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerAtRightEdge_WhenMovingRight_ThenPositionDoesNotChange()
    {
        //Arrange - player at right edge
        var player = new Player(StartPositions[StartPosition.RightEdge], 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has not moved
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerAtTopEdge_WhenMovingUp_ThenPositionDoesNotChange()
    {
        //Arrange - player at top edge
        var player = new Player(StartPositions[StartPosition.TopEdge], 100f, new FakeBoundaryChecker());

        //Act - move player up
        player.Move(MoveVectors[MoveDirection.Up]);

        //Assert - player has not moved
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenPlayerAtBottomEdge_WhenMovingDown_ThenPositionDoesNotChange()
    {
        //Arrange - player at bottom edge
        var player = new Player(StartPositions[StartPosition.BottomEdge], 100f, new FakeBoundaryChecker());

        //Act - move player down
        player.Move(MoveVectors[MoveDirection.Down]);

        //Assert - player has not moved
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(10);
    }

    [Fact]
    public void GivenPlayerAtTopEdge_WhenMovingRight_ThenMovesAlongEdge()
    {
        //Arrange - player at top edge
        var player = new Player(StartPositions[StartPosition.TopEdge], 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved right along the edge
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenPlayerAtLeftEdge_WhenMovingRight_ThenMovesAwayFromEdge()
    {
        //Arrange - player at left edge
        var player = new Player(StartPositions[StartPosition.LeftEdge], 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved away from the edge
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerWithinBounds_WhenMovingRight_ThenMovesWithinBounds()
    {
        //Arrange - player within bounds
        var player = new Player(StartPositions[StartPosition.Center], 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved within bounds
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerAtTopLeftCorner_WhenMovingDiagonallyUpLeft_ThenPositionDoesNotChange()
    {
        // Arrange - player at top-left corner
        var player = new Player(StartPositions[StartPosition.TopLeftCorner], 100f, new FakeBoundaryChecker());

        // Act - move player up and left
        player.Move(MoveVectors[MoveDirection.LeftUp]);

        // Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenPlayerWithinBounds_WhenMovingDiagonallyUpRight_ThenMovesDiagonally()
    {
        // Arrange - player within bounds
        var player = new Player(StartPositions[StartPosition.Center], 100f, new FakeBoundaryChecker());

        // Act - move player up and right
        player.Move(MoveVectors[MoveDirection.RightUp]);

        // Assert - player has moved diagonally
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void GivenPlayerWithinBounds_WhenNoDirectionGiven_ThenPositionDoesNotChange()
    {
        // Arrange - player within bounds
        var player = new Player(StartPositions[StartPosition.Center], 100f, new FakeBoundaryChecker());

        // Act - Move player with no direction
        player.Move(MoveVectors[MoveDirection.None]);

        // Assert - player has not moved
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerWithHealth_WhenTakingDamage_ThenHealthDecreases()
    {
        // Arrange - player with initial health
        var player = new Player(StartPositions[StartPosition.Center], 100f, new NullBoundaryChecker());
        player.IsDead.Should().BeFalse();

        // Act - player takes a hit
        player.TakeDamage(20f);

        // Assert - player's health has decreased
        player.Health.Should().Be(80f);
        player.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenPlayerWithLowHealth_WhenTakingExcessiveDamage_ThenHealthIsZeroAndIsDead()
    {
        // Arrange - player with initial health
        var player = new Player(StartPositions[StartPosition.Center], 10f, new NullBoundaryChecker());
        player.IsDead.Should().BeFalse();

        // Act - player takes a hit that exceeds current health
        player.TakeDamage(20f);

        // Assert - player's health is set to zero
        player.Health.Should().Be(0f);
        player.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenPlayerWithHealth_WhenTakingDamageEqualToHealth_ThenHealthIsZeroAndIsDead()
    {
        // Arrange - player with zero health
        var player = new Player(StartPositions[StartPosition.Center], 10f, new NullBoundaryChecker());
        player.IsDead.Should().BeFalse();

        // Act - player takes a hit that reduces health to zero
        player.TakeDamage(10f);

        // Assert - player is dead
        player.Health.Should().Be(0f);
        player.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenPlayerWithPositiveHealth_WhenCheckingIsDead_ThenReturnsFalse()
    {
        // Arrange - player with positive health
        var player = new Player(StartPositions[StartPosition.Center], 50f, new NullBoundaryChecker());

        // Assert - player is not dead
        player.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenPlayerWithHealth_WhenTakingNegativeDamage_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange - player with initial health
        var player = new Player(StartPositions[StartPosition.Center], 100f, new NullBoundaryChecker());

        // Act & Assert - player cannot take negative damage
        Action act = () => player.TakeDamage(-10f);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Damage cannot be negative.*")
            .WithParameterName("damage");
    }

    [Fact]
    public void GivenDeadPlayer_WhenTakingDamage_ThenHealthDoesNotChange()
    {
        // Arrange - player with zero health
        var player = new Player(StartPositions[StartPosition.Center], 10f, new NullBoundaryChecker());
        player.TakeDamage(10f);
        player.IsDead.Should().BeTrue();

        // Act - player tries to take damage
        player.TakeDamage(20f);

        // Assert - player cannot take damage when already dead
        player.Health.Should().Be(0f);
        player.IsDead.Should().BeTrue();
    }

    [Fact]
    public void GivenPlayerWithHealth_WhenTakingZeroDamage_ThenHealthDoesNotChange()
    {
        // Arrange - player with initial health
        var player = new Player(StartPositions[StartPosition.Center], 100f, new NullBoundaryChecker());
        player.IsDead.Should().BeFalse();

        // Act - player takes zero damage
        player.TakeDamage(0f);

        // Assert - player's health remains unchanged
        player.Health.Should().Be(100f);
        player.IsDead.Should().BeFalse();
    }

    [Fact]
    public void GivenPlayerWithHealth_WhenTakingMultipleHits_ThenHealthDecreasesCorrectly()
    {
        // Arrange - player with initial health
        var player = new Player(StartPositions[StartPosition.Center], 100f, new NullBoundaryChecker());
        player.IsDead.Should().BeFalse();

        // Act - player takes multiple hits
        player.TakeDamage(20f);
        player.TakeDamage(30f);
        player.TakeDamage(10f);

        // Assert - player's health has decreased correctly
        player.Health.Should().Be(40f);
        player.IsDead.Should().BeFalse();
    }

}