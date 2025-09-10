using System.Numerics;

namespace BattleStars.Core;

public interface IContext
{
    Vector2 PlayerDirection { get; }

    /// <summary>
    /// Position of the shooter in world coordinates.
    /// </summary>
    /// <remarks>
    /// Any concrete implementation must ensure that the ShooterPosition,
    /// is not NaN/Infinity and is a valid Vector2.
    /// </remarks>
    Vector2 ShooterPosition { get; set; }
}