using System.Numerics;
using BattleStars.Utility;
namespace BattleStars.Shapes;

public interface IShape : IContains<PositionalVector2>
{
    BoundingBox BoundingBox { get; }

    void Draw(PositionalVector2 position);
}