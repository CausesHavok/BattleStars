using System.Numerics;
namespace BattleStars;
public interface IShot
{
    Vector2 Position { get; }
    Vector2 Direction { get; }
    float Speed { get; }
    float Damage { get; }
    bool IsActive { get; }
    void Update();
}