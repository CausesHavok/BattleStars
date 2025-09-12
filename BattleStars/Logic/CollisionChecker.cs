using BattleStars.Shots;
using BattleStars.Core;
namespace BattleStars.Logic;

public static class CollisionChecker
{
    public static bool CheckEntityShotCollision(Entity entity, IShot shot)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(shot);
        var adjustedPosition = shot.Position - entity.Position;
        return entity.Shape.Contains(adjustedPosition);
    }

    public static bool CheckBattleStarShotCollision(BattleStar battleStar, IShot shot)
    {
        ArgumentNullException.ThrowIfNull(battleStar);
        ArgumentNullException.ThrowIfNull(shot);
 
        return battleStar.Contains(shot.Position);
    }
}
