using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

public interface IMovable
{
    void Move(IContext context);
    PositionalVector2 Position { get; }
}
