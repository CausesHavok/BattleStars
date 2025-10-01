using BattleStars.Domain.ValueObjects;
namespace BattleStars.Domain.Interfaces;

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