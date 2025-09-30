using BattleStars.Core;
using BattleStars.Shots;
namespace BattleStars.Logic;
public interface ICollisionChecker
{
    bool CheckBattleStarShotCollision(IBattleStar battleStar, IShot shot);
}