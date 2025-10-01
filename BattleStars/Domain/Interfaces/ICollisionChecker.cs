namespace BattleStars.Domain.Interfaces;
public interface ICollisionChecker
{
    bool CheckBattleStarShotCollision(IBattleStar battleStar, IShot shot);
}