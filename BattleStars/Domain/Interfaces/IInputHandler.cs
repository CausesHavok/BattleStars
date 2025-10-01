using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

/// <summary>
/// Interface for handling player input in the game.
/// </summary>
/// <remarks>
/// This interface abstracts the input handling mechanism, allowing for different implementations
/// </remarks>
public interface IInputHandler
{
    DirectionalVector2 GetMovement();   // Returns directional input (e.g., WASD or arrow keys)
    bool ShouldShoot();                 // Returns true if shoot key is pressed
    bool ShouldExit();                  // Returns true if exit key (e.g., Escape) is pressed
}
