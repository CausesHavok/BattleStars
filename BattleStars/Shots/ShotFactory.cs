using BattleStars.Utility;
namespace BattleStars.Shots;

public static class ShotFactory
{
    public static IShot CustomShot(PositionalVector2 position, DirectionalVector2 direction, float speed, float damage)
        => new Shot(position, direction, speed, damage);

    public static IShot CreateScatterShot(PositionalVector2 position, DirectionalVector2 direction)
        => new Shot(position, direction, speed: 3f, damage: 3f);

    public static IShot CreateSniperShot(PositionalVector2 position, DirectionalVector2 direction)
        => new Shot(position, direction, speed: 50f, damage: 15f);

    public static IShot CreateCannonShot(PositionalVector2 position, DirectionalVector2 direction)
        => new Shot(position, direction, speed: 2f, damage: 20f);

    public static IShot CreateLaserShot(PositionalVector2 position, DirectionalVector2 direction)
        => new Shot(position, direction, speed: 10f, damage: 3f);
}