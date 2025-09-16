using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public class BasicContext : IContext
{

    public DirectionalVector2 PlayerDirection { get; set; }

    /// <inheritdoc/>
    public PositionalVector2 ShooterPosition { get; set; }

}