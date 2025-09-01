using System.Numerics;

namespace BattleStars.Core;

public interface IMovable
{
    void Move(IContext context);
    Vector2 Position { get; }
}
