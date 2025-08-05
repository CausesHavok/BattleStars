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

    Func<Vector2, Vector2, IShot> testShotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);

    [Fact]
    public void GivenNullBoundaryChecker_WhenCreatingPlayer_ThenThrowsArgumentNullException()
    {
        // Arrange & Act
        #pragma warning disable CS8625
        Action act = () => new Player(StartPositions[StartPosition.Center], 100f, null, testShotFactory);
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
        var player = new Player(StartPositions[StartPosition.LeftEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

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
        var player = new Player(StartPositions[StartPosition.RightEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

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
        var player = new Player(StartPositions[StartPosition.TopEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

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
        var player = new Player(StartPositions[StartPosition.BottomEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

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
        var player = new Player(StartPositions[StartPosition.TopEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

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
        var player = new Player(StartPositions[StartPosition.LeftEdge], 100f, new FakeBoundaryChecker(), testShotFactory);

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved away from the edge
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void GivenPlayerAtTopLeftCorner_WhenMovingDiagonallyUpLeft_ThenPositionDoesNotChange()
    {
        // Arrange - player at top-left corner
        var player = new Player(StartPositions[StartPosition.TopLeftCorner], 100f, new FakeBoundaryChecker(), testShotFactory);

        // Act - move player up and left
        player.Move(MoveVectors[MoveDirection.LeftUp]);

        // Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(0);
    }



}