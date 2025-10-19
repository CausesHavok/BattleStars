using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

internal interface IShape : IContains<PositionalVector2>
{
    BoundingBox BoundingBox { get; }

    void Draw(PositionalVector2 position);
}