using System.Numerics;
using BattleStars.Domain.Interfaces;
using BattleStars.Infrastructure.Adapters;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;

namespace BattleStars.Application.Services;

/// <summary>
/// Handles user input using Raylib for a BattleStars game.
/// </summary>
/// <remarks>
/// This class translates keyboard input into game actions such as movement, shooting, and exiting.
/// It relies on an IKeyboardProvider to abstract away the specific input handling implementation,
/// enabling easier testing and flexibility.
/// </remarks>
public class InputHandler : IInputHandler
{
    private readonly IKeyboardProvider _keyboardProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="InputHandler"/> class.
    /// </summary>
    /// <param name="keyboardProvider">The keyboard provider to use for input handling.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyboardProvider"/> is null.</exception>
    public InputHandler(IKeyboardProvider keyboardProvider)
    {
        Guard.NotNull(keyboardProvider, nameof(keyboardProvider));
        _keyboardProvider = keyboardProvider;
    }

    /// <summary>
    /// Gets the movement direction based on current keyboard input.
    /// </summary>
    /// <returns>A <see cref="DirectionalVector2"/> representing the movement direction.</returns>
    /// <remarks>
    /// The direction is determined by the arrow keys:
    /// - Left Arrow: Move left
    /// - Right Arrow: Move right
    /// - Up Arrow: Move up
    /// - Down Arrow: Move down
    /// If no keys are pressed, returns <see cref="DirectionalVector2.Zero"/>.
    /// </remarks>
    public DirectionalVector2 GetMovement()
    {
        float x = 0;
        float y = 0;

        if (_keyboardProvider.IsKeyDown(GameKey.Left))
            x -= 1;
        if (_keyboardProvider.IsKeyDown(GameKey.Right))
            x += 1;
        if (_keyboardProvider.IsKeyDown(GameKey.Up))
            y -= 1;
        if (_keyboardProvider.IsKeyDown(GameKey.Down))
            y += 1;

        if (x == 0 && y == 0)
            return DirectionalVector2.Zero;

        var movement = new DirectionalVector2(Vector2.Normalize(new Vector2(x, y)));
        return movement;
    }

    /// <summary>
    /// Determines if the shoot action should be performed based on current keyboard input.
    /// </summary>
    /// <returns><c>true</c> if the shoot action should be performed; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The shoot action is triggered by holding down the Space key.
    /// </remarks>
    public bool ShouldShoot()
    {
        return _keyboardProvider.IsKeyDown(GameKey.Space);
    }

    /// <summary>
    /// Determines if the game should exit based on current keyboard input.
    /// </summary>
    /// <returns><c>true</c> if the game should exit; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The game exit action is triggered by pressing the Escape key.
    /// </remarks>
    public bool ShouldExit()
    {
        return _keyboardProvider.IsKeyPressed(GameKey.Escape);
    }
}