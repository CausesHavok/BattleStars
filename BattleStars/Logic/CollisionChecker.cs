using BattleStars.Shots;
using BattleStars.Core;
namespace BattleStars.Logic;

public static class CollisionChecker
{
    public static bool CheckBattleStarShotCollision(IBattleStar battleStar, IShot shot)
    {
        ArgumentNullException.ThrowIfNull(battleStar);
        ArgumentNullException.ThrowIfNull(shot);

        return battleStar.Contains(shot.Position);
    }
}
