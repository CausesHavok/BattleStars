using BattleStars.Domain.Entities;
using BattleStars.Domain.Interfaces;
using BattleStars.Domain.ValueObjects;
using BattleStars.Infrastructure.Utilities;
namespace BattleStars.Infrastructure.Factories;

public static class ShotFactory
{
    private static void ValidateShotParameters(float speed, float damage)
    {
        FloatValidator.ThrowIfNaNOrInfinity(speed, nameof(speed));
        FloatValidator.ThrowIfNaNOrInfinity(damage, nameof(damage));
        FloatValidator.ThrowIfNegative(speed, nameof(speed));
        FloatValidator.ThrowIfNegative(damage, nameof(damage));
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