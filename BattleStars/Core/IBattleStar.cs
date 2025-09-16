using System.Numerics;
using BattleStars.Shapes;
using BattleStars.Shots;
using BattleStars.Utility;

namespace BattleStars.Core;

public interface IBattleStar
{
    bool IsDestroyed { get; }

    bool Contains(PositionalVector2 point);
    void Draw();
    BoundingBox GetBoundingBox();
    void Move(IContext context);
    IEnumerable<IShot> Shoot(IContext context);
    void TakeDamage(float amount);
}