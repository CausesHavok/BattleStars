namespace BattleStars.Logic;

/// <summary>
/// Interface for keyboard input handling.
/// </summary>
/// <remarks>
/// This interface abstracts the keyboard input mechanism, allowing for different implementations
/// (e.g., Raylib, custom input systems).
/// </remarks>
public interface IKeyboardProvider
{
    bool IsKeyDown(GameKey key);
    bool IsKeyPressed(GameKey key);
}