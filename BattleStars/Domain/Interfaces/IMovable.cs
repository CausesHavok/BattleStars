using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

internal interface IMovable
{
    void Move(IContext context);
    PositionalVector2 Position { get; }
}
