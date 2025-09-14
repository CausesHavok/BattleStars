using System.Numerics;
using BattleStars.Shapes;
using BattleStars.Shots;

namespace BattleStars.Core;

public interface IBattleStar
{
    bool IsDestroyed { get; }

    bool Contains(Vector2 point);
    void Draw();
    BoundingBox GetBoundingBox();
    void Move(IContext context);
    IEnumerable<IShot> Shoot(IContext context);
    void TakeDamage(float amount);
}