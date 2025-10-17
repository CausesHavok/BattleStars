using BattleStars.Domain.Interfaces;
using BattleStars.Core.Guards;
namespace BattleStars.Application.Checkers;

public class CollisionChecker : ICollisionChecker
{
    public bool CheckBattleStarShotCollision(IBattleStar battleStar, IShot shot)
    {
        Guard.NotNull(battleStar, nameof(battleStar));
        Guard.NotNull(shot, nameof(shot));

        return battleStar.Contains(shot.Position);
    }
}
