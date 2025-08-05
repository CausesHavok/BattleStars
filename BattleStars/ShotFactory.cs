using System.Numerics;

namespace BattleStars;

public static class ShotFactory
{
    public static IShot CustomShot(Vector2 position, Vector2 direction, float speed, float damage)
    {
        return new Shot(position, direction, speed, damage);
    }

    public static IShot CreateScatterShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 5f, damage: 3f);

    public static IShot CreateSniperShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 20f, damage: 15f);

    public static IShot CreateCannonShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 6f, damage: 20f);

    public static IShot CreateLaserShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 20f, damage: 3f);
}