using BattleStars.Domain.Entities;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Core.Guards;
namespace BattleStars.Infrastructure.Factories;

public static class ShotFactory
{
    private static void ValidateShotParameters(float speed, float damage)
    {
        FloatGuard.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatGuard.ThrowIfNaNOrInfinity(damage, nameof(damage));
        FloatGuard.ThrowIfNegative(speed, nameof(speed));
        FloatGuard.ThrowIfNegative(damage, nameof(damage));
    }

    public static IShot CustomShot(PositionalVector2 position, DirectionalVector2 direction, float speed, float damage)
    {
        ValidateShotParameters(speed, damage);
        return new Shot(position, direction, speed, damage);
    }

    public static IShot CreateScatterShot(PositionalVector2 position, DirectionalVector2 direction)
        => CustomShot(position, direction, speed: 3f, damage: 3f);

    public static IShot CreateSniperShot(PositionalVector2 position, DirectionalVector2 direction)
        => CustomShot(position, direction, speed: 50f, damage: 15f);

    public static IShot CreateCannonShot(PositionalVector2 position, DirectionalVector2 direction)
        => CustomShot(position, direction, speed: 2f, damage: 20f);

    public static IShot CreateLaserShot(PositionalVector2 position, DirectionalVector2 direction)
        => CustomShot(position, direction, speed: 10f, damage: 3f);

    public static IShot CreateNoOpShot()
        => CustomShot(new PositionalVector2(0, 0), new DirectionalVector2(0, 0), speed: 0f, damage: 0f);
    
    public static IShot CreateNoOpShot(PositionalVector2 position)
        => CustomShot(position, new DirectionalVector2(0, 0), speed: 0f, damage: 0f);

    public static List<IShot> CreateEmptyShotList() => [];
}