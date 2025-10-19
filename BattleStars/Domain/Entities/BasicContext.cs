using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Entities;

internal class BasicContext : IContext
{

    public DirectionalVector2 PlayerDirection { get; set; }

    /// <inheritdoc/>
    public PositionalVector2 ShooterPosition { get; set; }

}