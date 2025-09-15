using System.Numerics;
using BattleStars.Utility;
namespace BattleStars.Shots;
public interface IShot
{
    PositionalVector2 Position { get; }
    DirectionalVector2 Direction { get; }
    float Speed { get; }
    float Damage { get; }
    bool IsActive { get; }
    void Update();
}