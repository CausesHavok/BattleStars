using BattleStars.Logic;
using BattleStars.Utility;
using FluentAssertions;

namespace BattleStars.Tests.Logic;

public class InputHandlerTest
{
    private class MockKeyboardProvider : IKeyboardProvider
    {
        public HashSet<GameKey> DownKeys { get; } = new();
        public HashSet<GameKey> PressedKeys { get; } = new();

        public bool IsKeyDown(GameKey key) => DownKeys.Contains(key);
        public bool IsKeyPressed(GameKey key) => PressedKeys.Contains(key);
    }

    [Fact]
    public void GivenNullKeyboardProvider_WhenConstructing_ThenThrowsArgumentNullException()
    {
        Action act = () => new InputHandler(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GivenNoKeysDown_WhenGetMovementIsCalled_ThenReturnsZeroVector()
    {
        var mock = new MockKeyboardProvider();
        var handler = new InputHandler(mock);

        var movement = handler.GetMovement();

        movement.Should().Be(DirectionalVector2.Zero);
    }

    [Theory]
    [InlineData(GameKey.Left, -1, 0)]
    [InlineData(GameKey.Right, 1, 0)]
    [InlineData(GameKey.Up, 0, -1)]
    [InlineData(GameKey.Down, 0, 1)]
    public void GivenSingleDirectionKeyDown_WhenGetMovementIsCalled_ThenReturnsCorrectUnitVector(GameKey key, float expectedX, float expectedY)
    {
        var mock = new MockKeyboardProvider();
        mock.DownKeys.Add(key);
        var handler = new InputHandler(mock);

        var movement = handler.GetMovement();

        movement.X.Should().BeApproximately(expectedX, 0.001f);
        movement.Y.Should().BeApproximately(expectedY, 0.001f);
    }

    [Theory]
    [InlineData(GameKey.Left, GameKey.Right)]
    [InlineData(GameKey.Up, GameKey.Down)]
    public void GivenOppositeDirectionKeysDown_WhenGetMovementIsCalled_ThenReturnsZeroVector(GameKey key1, GameKey key2)
    {
        var mock = new MockKeyboardProvider();
        mock.DownKeys.Add(key1);
        mock.DownKeys.Add(key2);
        var handler = new InputHandler(mock);

        var movement = handler.GetMovement();

        movement.Should().Be(DirectionalVector2.Zero);
    }

    [Theory]
    [InlineData(GameKey.Left, GameKey.Up, -0.707f, -0.707f)]
    [InlineData(GameKey.Left, GameKey.Down, -0.707f, 0.707f)]
    [InlineData(GameKey.Right, GameKey.Up, 0.707f, -0.707f)]
    [InlineData(GameKey.Right, GameKey.Down, 0.707f, 0.707f)]
    public void GivenTwoDirectionKeysDown_WhenGetMovementIsCalled_ThenReturnsCorrectNormalizedVector(GameKey key1, GameKey key2, float expectedX, float expectedY)
    {
        var mock = new MockKeyboardProvider();
        mock.DownKeys.Add(key1);
        mock.DownKeys.Add(key2);
        var handler = new InputHandler(mock);

        var movement = handler.GetMovement();

        movement.X.Should().BeApproximately(expectedX, 0.001f);
        movement.Y.Should().BeApproximately(expectedY, 0.001f);
    }

    [Fact]
    public void GivenSpaceKeyDown_WhenShouldShootIsCalled_ThenReturnsTrue()
    {
        var mock = new MockKeyboardProvider();
        mock.DownKeys.Add(GameKey.Space);
        var handler = new InputHandler(mock);

        handler.ShouldShoot().Should().BeTrue();
    }

    [Fact]
    public void GivenSpaceKeyNotDown_WhenShouldShootIsCalled_ThenReturnsFalse()
    {
        var mock = new MockKeyboardProvider();
        var handler = new InputHandler(mock);

        handler.ShouldShoot().Should().BeFalse();
    }

    [Fact]
    public void GivenEscapeKeyPressed_WhenShouldExitIsCalled_ThenReturnsTrue()
    {
        var mock = new MockKeyboardProvider();
        mock.PressedKeys.Add(GameKey.Escape);
        var handler = new InputHandler(mock);

        handler.ShouldExit().Should().BeTrue();
    }

    [Fact]
    public void GivenEscapeKeyNotPressed_WhenShouldExitIsCalled_ThenReturnsFalse()
    {
        var mock = new MockKeyboardProvider();
        var handler = new InputHandler(mock);

        handler.ShouldExit().Should().BeFalse();
    }
}