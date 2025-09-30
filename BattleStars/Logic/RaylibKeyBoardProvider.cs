using Raylib_cs;
namespace BattleStars.Logic;

/// <summary>
/// Implementation of IKeyboardProvider using Raylib for keyboard input handling.
/// </summary>
/// <remarks>
/// This class is a super thin wrapper around Raylib's keyboard functions,
/// allowing the rest of the game logic to remain decoupled from Raylib.
/// It is marked to be excluded from code coverage as it directly wraps external library calls.
/// </remarks>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class RaylibKeyBoardProvider : IKeyboardProvider
{
    /// <summary>
    /// Checks if a specific key is currently being pressed down.
    /// </summary>
    /// <param name="key">The game key to check.</param>
    /// <returns>True if the key is currently pressed down, false otherwise.</returns>
    public bool IsKeyDown(GameKey key) => Raylib.IsKeyDown(MapKey(key));

    /// <summary>
    /// Checks if a specific key was pressed (transitioned from up to down) since the last frame.
    /// </summary>
    /// <param name="key">The game key to check.</param>
    /// <returns>True if the key was pressed, false otherwise.</returns>
    public bool IsKeyPressed(GameKey key) => Raylib.IsKeyPressed(MapKey(key));

    /// <summary>
    /// Maps a GameKey to the corresponding Raylib KeyboardKey.
    /// </summary>
    /// <param name="key">The game key to map.</param>
    /// <returns>The corresponding Raylib KeyboardKey.</returns>
    private KeyboardKey MapKey(GameKey key) => key switch
    {
        GameKey.Left => KeyboardKey.Left,
        GameKey.Right => KeyboardKey.Right,
        GameKey.Up => KeyboardKey.Up,
        GameKey.Down => KeyboardKey.Down,
        GameKey.Space => KeyboardKey.Space,
        GameKey.Escape => KeyboardKey.Escape,
        _ => throw new ArgumentOutOfRangeException(nameof(key))
    };
}
