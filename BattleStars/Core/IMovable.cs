using System.Numerics;
using BattleStars.Utility;

namespace BattleStars.Core;

public interface IMovable
{
    void Move(IContext context);
    PositionalVector2 Position { get; }
}
