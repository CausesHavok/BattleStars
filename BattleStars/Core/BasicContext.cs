using System.Numerics;

namespace BattleStars.Core;

public class BasicContext : IContext
{
    
    public Vector2 PlayerDirection { get; set; }

    /// <inheritdoc/>
    public Vector2 ShooterPosition { get; set; }

}