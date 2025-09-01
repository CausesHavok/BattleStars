using System.Numerics;

namespace BattleStars.Core;

public interface IContext
{
    Vector2 PlayerDirection { get; }
}