using System.Numerics;
using FluentAssertions;

namespace BattleStars.Tests;

public class PlayerTest
{
    public enum MoveDirection
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


    [Fact]
    public void Player_Cannot_Move_Beyond_Left_Edge()
    {
        //Arrange - player at left edge
        var player = new Player(new Vector2(0, 0), 100f, new FakeBoundaryChecker());

        //Act - move player left
        player.Move(MoveVectors[MoveDirection.Left]);

        //Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void Player_Cannot_Move_Beyond_Right_Edge()
    {
        //Arrange - player at right edge
        var player = new Player(new Vector2(10, 0), 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has not moved
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void Player_Cannot_Move_Beyond_Top_Edge()
    {
        //Arrange - player at top edge
        var player = new Player(new Vector2(0, 0), 100f, new FakeBoundaryChecker());

        //Act - move player up
        player.Move(MoveVectors[MoveDirection.Up]);

        //Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void Player_Cannot_Move_Beyond_Bottom_Edge()
    {
        //Arrange - player at bottom edge
        var player = new Player(new Vector2(0, 10), 100f, new FakeBoundaryChecker());

        //Act - move player down
        player.Move(MoveVectors[MoveDirection.Down]);

        //Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(10);
    }

    [Fact]
    public void Player_Can_Move_Along_Edge()
    {
        //Arrange - player at left edge
        var player = new Player(new Vector2(0, 0), 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved right along the edge
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void Player_Can_Move_Away_From_Edge()
    {
        //Arrange - player at left edge
        var player = new Player(new Vector2(0, 5), 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved away from the edge
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(5);
    }


    [Fact]
    public void Player_Can_Move_Within_Bounds()
    {
        //Arrange - player within bounds
        var player = new Player(new Vector2(5, 5), 100f, new FakeBoundaryChecker());

        //Act - move player right
        player.Move(MoveVectors[MoveDirection.Right]);

        //Assert - player has moved within bounds
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(5);
    }


    [Fact]
    public void Player_Cannot_Move_Diagonally_Beyond_TopLeft_Edge()
    {
        // Arrange - player at top-left corner
        var player = new Player(new Vector2(0, 10), 100f, new FakeBoundaryChecker());

        // Act - move player up and left
        player.Move(MoveVectors[MoveDirection.LeftUp]);

        // Assert - player has not moved
        player.Position.X.Should().Be(0);
        player.Position.Y.Should().Be(5);
    }

    [Fact]
    public void Player_Can_Move_Diagonally_Within_Bounds()
    {
        // Arrange - player within bounds
        var player = new Player(new Vector2(5, 5), 100f, new FakeBoundaryChecker());

        // Act - move player up and right
        player.Move(MoveVectors[MoveDirection.RightUp]);

        // Assert - player has moved diagonally
        player.Position.X.Should().Be(10);
        player.Position.Y.Should().Be(0);
    }

    [Fact]
    public void Player_Does_Not_Move_When_Not_Directed_To()
    {
        // Arrange - player within bounds
        var player = new Player(new Vector2(5, 5), 100f, new FakeBoundaryChecker());

        // Act - Move player with no direction
        player.Move(MoveVectors[MoveDirection.None]);

        // Assert - player has not moved
        player.Position.X.Should().Be(5);
        player.Position.Y.Should().Be(5);
    }
}
