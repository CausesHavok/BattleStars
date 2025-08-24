using System.Numerics;
namespace BattleStars;

public static class ShotFactory
{
    public static IShot CustomShot(Vector2 position, Vector2 direction, float speed, float damage)
        => new Shot(position, direction, speed, damage);

    public static IShot CreateScatterShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 3f, damage: 3f);

    public static IShot CreateSniperShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 50f, damage: 15f);

    public static IShot CreateCannonShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 2f, damage: 20f);

    public static IShot CreateLaserShot(Vector2 position, Vector2 direction)
        => new Shot(position, direction, speed: 10f, damage: 3f);
}