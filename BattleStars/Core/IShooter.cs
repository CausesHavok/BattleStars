using BattleStars.Shots;

namespace BattleStars.Core;

public interface IShooter
{
    IEnumerable<IShot> Shoot(IContext context);
}