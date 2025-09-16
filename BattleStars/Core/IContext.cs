using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public interface IContext
{
    DirectionalVector2 PlayerDirection { get; }

    /// <summary>
    /// Position of the shooter in world coordinates.
    /// </summary>
    /// <remarks>
    /// Any concrete implementation must ensure that the ShooterPosition,
    /// is not NaN/Infinity and is a valid Vector2.
    /// </remarks>
    PositionalVector2 ShooterPosition { get; set; }
}