using System.Numerics;
using BattleStars.Shapes;
using FluentAssertions;

namespace BattleStars.Tests;

public class PlayerTest
{

    public enum StartPosition
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
        { StartPosition.LeftEdge,           new Vector2(-5,  0) },
        { StartPosition.RightEdge,          new Vector2( 5,  0) },
        { StartPosition.TopEdge,            new Vector2( 0, -5) },
        { StartPosition.BottomEdge,         new Vector2( 0,  5) },
        { StartPosition.Center,             new Vector2( 0,  0) },
        { StartPosition.TopLeftCorner,      new Vector2(-5, -5) },
        { StartPosition.TopRightCorner,     new Vector2( 5, -5) },
        { StartPosition.BottomLeftCorner,   new Vector2(-5,  5) },
        { StartPosition.BottomRightCorner,  new Vector2( 5,  5) }
    };

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
        { MoveDirection.None,                           new Vector2( 0,  0)  },
        { MoveDirection.Left,                           new Vector2(-1,  0)  },
        { MoveDirection.Right,                          new Vector2( 1,  0)  },
        { MoveDirection.Up,                             new Vector2( 0, -1)  },
        { MoveDirection.Down,                           new Vector2( 0,  1)  },
        { MoveDirection.LeftUp,     Vector2.Normalize(  new Vector2(-1, -1)) },
        { MoveDirection.LeftDown,   Vector2.Normalize(  new Vector2(-1,  1)) },
        { MoveDirection.RightUp,    Vector2.Normalize(  new Vector2( 1, -1)) },
        { MoveDirection.RightDown,  Vector2.Normalize(  new Vector2( 1,  1)) }
    };

    private class TestBoundaryChecker : IBoundaryChecker
    {
        public bool IsOutsideXBounds(float x)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return x < -5 || x > 5;
        }

        public float XDistanceToBoundary(float x)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return Math.Min(Math.Abs(x + 5), Math.Abs(x - 5));
        }

        public bool IsOutsideYBounds(float y)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return y < -5 || y > 5;
        }

        public float YDistanceToBoundary(float y)
        {
            // For testing purposes, let's assume the bounds are within a 10x10 square
            return Math.Min(Math.Abs(y + 5), Math.Abs(y - 5));
        }
    }

    private class NullBoundaryChecker : IBoundaryChecker
    {
        public bool IsOutsideXBounds(float x) => false;
        public bool IsOutsideYBounds(float y) => false;
        public float XDistanceToBoundary(float x) => 0;
        public float YDistanceToBoundary(float y) => 0;
    }

    private class TestSmallBoundaryChecker : IBoundaryChecker
    {
        public bool IsOutsideXBounds(float x) => x < -0.5f || x > 0.5f;
        public float XDistanceToBoundary(float x) => Math.Min(Math.Abs(x + 0.5f), Math.Abs(x - 0.5f));
        public bool IsOutsideYBounds(float y) => y < -0.5f || y > 0.5f;
        public float YDistanceToBoundary(float y) => Math.Min(Math.Abs(y + 0.5f), Math.Abs(y - 0.5f));
    }

    Func<Vector2, Vector2, IShot> testShotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);
    private TestBoundaryChecker _testBoundaryChecker = new TestBoundaryChecker();
    private NullBoundaryChecker _nullBoundaryChecker = new NullBoundaryChecker();
    private TestSmallBoundaryChecker _testSmallBoundaryChecker = new TestSmallBoundaryChecker();
    private Circle _cirle = new Circle(10f, System.Drawing.Color.Red);

    private Player CreateMovementTestPlayer(Vector2 position, IBoundaryChecker boundaryChecker)
    {
        return new Player(position, 100f, boundaryChecker, testShotFactory, _cirle);
    }

    #region Player Creation Tests
    /* Test cases for Player creation and initialization
        - Ensure Player constructor initializes position, health, boundary checker, shot factory, and shape correctly
            Player is initialized with correct position
            Player is initialized with correct health
            Player is initialized with correct shape
        - Ensure Player constructor throws exception when input is null
            Player constructor throws ArgumentNullException when boundary checker is null
            Player constructor throws ArgumentNullException when shot factory is null
            Player constructor throws ArgumentNullException when shape is null
    */

    // - Ensure Player constructor initializes position, health, boundary checker, shot factory, and shape correctly
    [Fact]
    public void GivenPlayer_WhenConstructed_InitializesCorrectly()
    {
        // Arrange
        var position = new Vector2(1, 2);
        var health = 100f;
        var boundaryChecker = new NullBoundaryChecker();
        var shotFactory = testShotFactory;
        var shape = _cirle;

        // Act
        var player = new Player(position, health, boundaryChecker, shotFactory, shape);

        // Assert
        player.Position.Should().Be(position);
        player.Health.Should().Be(health);
        player.IsDead.Should().BeFalse();
        player.Shape.Should().Be(shape);
    }

    // - Ensure Player constructor throws exception when input is null
    [Fact]
    public void GivenPlayer_WhenConstructedWithNullBoundaryChecker_ThrowsArgumentNullException()
    {
        // Arrange
        Func<Vector2, Vector2, IShot> shotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);

        // Act
        Action act = () => new Player(new Vector2(0, 0), 100f, null!, shotFactory, _cirle);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Boundary checker cannot be null.*")
            .And.ParamName.Should().Be("boundaryChecker");
    }

    [Fact]
    public void GivenPlayer_WhenConstructedWithNullShotFactory_ThrowsArgumentNullException()
    {
        // Arrange
        IBoundaryChecker boundaryChecker = new NullBoundaryChecker();

        // Act
        Action act = () => new Player(new Vector2(0, 0), 100f, boundaryChecker, null!, _cirle);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Shot factory cannot be null.*")
            .And.ParamName.Should().Be("shotFactory");
    }

    [Fact]
    public void GivenPlayer_WhenConstructedWithNullShape_ThrowsArgumentNullException()
    {
        // Arrange
        IBoundaryChecker boundaryChecker = new NullBoundaryChecker();
        Func<Vector2, Vector2, IShot> shotFactory = (pos, dir) => new Shot(pos, dir, 1f, 1f);

        // Act
        Action act = () => new Player(new Vector2(0, 0), 100f, boundaryChecker, shotFactory, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Shape cannot be null.*")
            .And.ParamName.Should().Be("shape");
    }

    #endregion


    #region Player Movement Tests
    /*    Test cases for Player movement and boundary checking
        - Ensure Move method does nothing when player is dead
            Player with zero health does not move
        - Ensure Move method does nothing when direction is zero
            Player with zero direction does not move
        - Ensure Move method throws exception when direction is NaN or Infinity
            Player with NaN direction throws exception
            Player with Infinity direction throws exception
        - Ensure player does not move outside the defined boundaries
            Player on Left edge does not move left
            Player on Right edge does not move right
            Player on Top edge does not move up
            Player on Bottom edge does not move down
            Player on LeftUp corner does not move left or up
            Player on LeftDown corner does not move left or down
            Player on RightUp corner does not move right or up
            Player on RightDown corner does not move right or down
        - Ensure player can move along edges without crossing boundaries
            Player on Left edge can move up or down
            Player on Right edge can move up or down
            Player on Top edge can move left or right
            Player on Bottom edge can move right or left
            Player on LeftUp corner can move down or right
            Player on LeftDown corner can move up or right
            Player on RightUp corner can move down or left
            Player on RightDown corner can move up or left
        - Ensure player can move away from edges when at the boundary
            Player on Left edge moves right
            Player on Right edge moves left
            Player on Top edge moves down
            Player on Bottom edge moves up
            Player on LeftUp corner moves down or right
            Player on LeftDown corner moves up or right
            Player on RightUp corner moves down or left
            Player on RightDown corner moves up or left
        - Ensure player does not overshoot boundary
            Player not on left edge does not overshoot left boundary
            Player not on right edge does not overshoot right boundary
            Player not on top edge does not overshoot top boundary
            Player not on bottom edge does not overshoot bottom boundary
        - Ensure player on the inside can move within this area
            Player can move left
            Player can move right
            Player can move up
            Player can move down
            Player can move diagonally left-up
            Player can move diagonally left-down
            Player can move diagonally right-up
            Player can move diagonally right-down
        - Ensure players movement is projected correctly when moving diagonally across boundaries
            Player on left edge moving diagonally left-up does not move
            Player on left edge moving diagonally left-down does not move
            Player on right edge moving diagonally right-down does not move
            Player on right edge moving diagonally right-up does not move
            Player on top edge moving diagonally left-up does not move
            Player on top edge moving diagonally right-up does not move
            Player on bottom edge moving diagonally left-down does not move
            Player on bottom edge moving diagonally right-down does not move
    */

    // - Ensure Move method does nothing when player is dead
    [Fact]
    public void GivenPlayer_WhenDead_DoesNotMove()
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[StartPosition.Center], _nullBoundaryChecker);
        player.TakeDamage(100f); // Make the player dead
        var initialPosition = player.Position;

        // Act
        player.Move(new Vector2(5, 0)); // Attempt to move

        // Assert
        player.Position.Should().Be(initialPosition);
    }


    // - Ensure Move method does nothing when direction is zero
    [Fact]
    public void GivenPlayer_WhenMovingWithZeroDirection_DoesNotMove()
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[StartPosition.Center], _nullBoundaryChecker);
        var direction = Vector2.Zero;

        // Act
        player.Move(direction);

        // Assert
        player.Position.Should().Be(StartPositions[StartPosition.Center]);
    }

    // - Ensure Move method throws exception when direction is NaN or Infinity
    [Theory]
    [InlineData(float.NaN,              0f,                     "direction.X", "NaN.*")]
    [InlineData(float.PositiveInfinity, 0f,                     "direction.X", "Infinity.*")]
    [InlineData(float.NegativeInfinity, 0f,                     "direction.X", "Infinity.*")]
    [InlineData(0f,                     float.NaN,              "direction.Y", "NaN.*")]
    [InlineData(0f,                     float.PositiveInfinity, "direction.Y", "Infinity.*")]
    [InlineData(0f,                     float.NegativeInfinity, "direction.Y", "Infinity.*")]
    public void GivenPlayer_WhenMovingWithNaNOrInfinityDirection_ThrowsArgumentException(float inputX, float inputY, string expectedParamName, string expectedErrorMessage)
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[StartPosition.Center], _nullBoundaryChecker);
        var direction = new Vector2(inputX, inputY);

        // Act
        Action act = () => player.Move(direction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedParamName + " cannot be " + expectedErrorMessage)
            .And.ParamName.Should().Be(expectedParamName);
    }


    // - Ensure player does not move outside the defined boundaries
    [Theory]
    [InlineData(StartPosition.LeftEdge, MoveDirection.Left)]
    [InlineData(StartPosition.RightEdge, MoveDirection.Right)]
    [InlineData(StartPosition.TopEdge, MoveDirection.Up)]
    [InlineData(StartPosition.BottomEdge, MoveDirection.Down)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.LeftUp)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.Left)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.Up)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.RightUp)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.Right)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.Up)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.LeftDown)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.Left)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.Down)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.RightDown)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.Right)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.Down)]
    public void GivenPlayerOnEdge_WhenMovingOutsideBoundary_ThePlayerDoesNotMove(StartPosition startPosition, MoveDirection moveDirection)
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[startPosition], _testBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.Should().Be(StartPositions[startPosition]);
    }


    // - Ensure player can move along edges without crossing boundaries
    [Theory]
    [InlineData(StartPosition.LeftEdge, MoveDirection.Up)]
    [InlineData(StartPosition.LeftEdge, MoveDirection.Down)]
    [InlineData(StartPosition.RightEdge, MoveDirection.Up)]
    [InlineData(StartPosition.RightEdge, MoveDirection.Down)]
    [InlineData(StartPosition.TopEdge, MoveDirection.Left)]
    [InlineData(StartPosition.TopEdge, MoveDirection.Right)]
    [InlineData(StartPosition.BottomEdge, MoveDirection.Left)]
    [InlineData(StartPosition.BottomEdge, MoveDirection.Right)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.Down)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.Right)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.Left)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.Down)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.Right)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.Up)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.Left)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.Up)]
    public void GivenPlayerOnEdge_WhenMovingAlongEdge_ThePlayerMovesCorrectly(StartPosition startPosition, MoveDirection moveDirection)
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[startPosition], _testBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.Should().Be(StartPositions[startPosition] + moveVector);
    }

    // - Ensure player can move away from edges when at the boundary
    [Theory]
    [InlineData(StartPosition.LeftEdge, MoveDirection.Right)]
    [InlineData(StartPosition.RightEdge, MoveDirection.Left)]
    [InlineData(StartPosition.TopEdge, MoveDirection.Down)]
    [InlineData(StartPosition.BottomEdge, MoveDirection.Up)]
    [InlineData(StartPosition.TopLeftCorner, MoveDirection.RightDown)]
    [InlineData(StartPosition.TopRightCorner, MoveDirection.LeftDown)]
    [InlineData(StartPosition.BottomLeftCorner, MoveDirection.RightUp)]
    [InlineData(StartPosition.BottomRightCorner, MoveDirection.LeftUp)]
    public void GivenPlayerAtBoundary_WhenMovingAway_ThePlayerMovesCorrectly(StartPosition startPosition, MoveDirection moveDirection)
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[startPosition], _testBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.Should().Be(StartPositions[startPosition] + moveVector);
    }

    // - Ensure player does not overshoot boundary
    [Theory]
    [InlineData(MoveDirection.Left,         -0.5f,  0.0f)]
    [InlineData(MoveDirection.Right,         0.5f,  0.0f)]
    [InlineData(MoveDirection.Up,            0.0f, -0.5f)]
    [InlineData(MoveDirection.Down,          0.0f,  0.5f)]
    [InlineData(MoveDirection.LeftUp,       -0.5f, -0.5f)]
    [InlineData(MoveDirection.LeftDown,     -0.5f,  0.5f)]
    [InlineData(MoveDirection.RightUp,       0.5f, -0.5f)]
    [InlineData(MoveDirection.RightDown,     0.5f,  0.5f)]
    public void GivenPlayerNotOnEdge_WhenMovingOutsideBoundary_ThePlayerMovesToBoundary(MoveDirection moveDirection, float expectedX, float expectedY)
    {
        // Arrange
        var startPositionVector = StartPositions[StartPosition.Center];
        var player = CreateMovementTestPlayer(startPositionVector, _testSmallBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.X.Should().Be(expectedX);
        player.Position.Y.Should().Be(expectedY);
    }

    // - Ensure player on the inside can move within this area
    [Theory]
    [InlineData(MoveDirection.Left)]
    [InlineData(MoveDirection.Right)]
    [InlineData(MoveDirection.Up)]
    [InlineData(MoveDirection.Down)]
    [InlineData(MoveDirection.LeftUp)]
    [InlineData(MoveDirection.LeftDown)]
    [InlineData(MoveDirection.RightUp)]
    [InlineData(MoveDirection.RightDown)]
    public void GivenPlayerInCenter_WhenMoving_ThePlayerMovesCorrectly(MoveDirection moveDirection)
    {
        // Arrange
        var startPosition = StartPosition.Center;
        var player = CreateMovementTestPlayer(StartPositions[startPosition], _testBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.Should().Be(StartPositions[startPosition] + moveVector);
    }

    // - Ensure players movement is projected correctly when moving diagonally across boundaries
    [Theory]
    [InlineData(StartPosition.LeftEdge,     MoveDirection.LeftUp,    -5.0f, -0.7f)]
    [InlineData(StartPosition.LeftEdge,     MoveDirection.LeftDown,  -5.0f,  0.7f)]
    [InlineData(StartPosition.RightEdge,    MoveDirection.RightUp,    5.0f, -0.7f)]
    [InlineData(StartPosition.RightEdge,    MoveDirection.RightDown,  5.0f,  0.7f)]
    [InlineData(StartPosition.TopEdge,      MoveDirection.LeftUp,    -0.7f, -5.0f)]
    [InlineData(StartPosition.TopEdge,      MoveDirection.RightUp,    0.7f, -5.0f)]
    [InlineData(StartPosition.BottomEdge,   MoveDirection.LeftDown,  -0.7f,  5.0f)]
    [InlineData(StartPosition.BottomEdge,   MoveDirection.RightDown,  0.7f,  5.0f)]
    public void GivenPlayerOnEdge_WhenMovingDiagonally_ThePlayerMoveAlongEdge(StartPosition startPosition, MoveDirection moveDirection, float expectedX, float expectedY)
    {
        // Arrange
        var player = CreateMovementTestPlayer(StartPositions[startPosition], _testBoundaryChecker);
        var moveVector = MoveVectors[moveDirection];

        // Act
        player.Move(moveVector);

        // Assert
        player.Position.X.Should().BeApproximately(expectedX, 0.1f);
        player.Position.Y.Should().BeApproximately(expectedY, 0.1f);
    }

    #endregion


}