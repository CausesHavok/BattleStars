using System.Numerics;

namespace BattleStars;

public static class CollisionChecker
{
    public static bool CheckEntityEntityCollision(Entity entity1, Entity entity2)
    {
        return false; // Placeholder for actual collision logic
    }

    public static bool CheckEntityShotCollision(Entity entity, IShot shot)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(shot);
        return entity.Shape.Contains(shot.Position, entity.Position);
    }

    public static bool CheckShotShotCollision(IShot shot1, IShot shot2)
    {
        return false; // Placeholder for actual collision logic
    }
}
