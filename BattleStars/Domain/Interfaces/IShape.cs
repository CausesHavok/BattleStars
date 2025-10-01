using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

public interface IShape : IContains<PositionalVector2>
{
    BoundingBox BoundingBox { get; }

    void Draw(PositionalVector2 position);
}