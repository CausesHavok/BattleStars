using BattleStars.Domain.Interfaces;
namespace BattleStars.Application.Checkers;

public class CollisionChecker : ICollisionChecker
{
    public bool CheckBattleStarShotCollision(IBattleStar battleStar, IShot shot)
    {
        ArgumentNullException.ThrowIfNull(battleStar);
        ArgumentNullException.ThrowIfNull(shot);

        return battleStar.Contains(shot.Position);
    }
}
