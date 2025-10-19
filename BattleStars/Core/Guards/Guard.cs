using System.Numerics;

namespace BattleStars.Core.Guards;

public static class Guard
{
    public static T NotNull<T>(this T? value, string name) where T : class
        => NullGuard.NotNull(value, name);

    public static float RequireNotNaN(float value, string paramName)
        => FloatGuard.RequireNotNaN(value, paramName);

    public static float RequireFinite(float value, string paramName)
        => FloatGuard.RequireFinite(value, paramName);

    public static float RequireNonNegative(float value, string paramName)
        => FloatGuard.RequireNonNegative(value, paramName);

    public static float RequireNonZero(float value, string paramName)
        => FloatGuard.RequireNonZero(value, paramName);

    public static float RequirePositive(float value, string paramName)
        => FloatGuard.RequirePositive(value, paramName);

    public static float RequireValid(float value, string paramName)
        => FloatGuard.RequireValid(value, paramName);

    public static Vector2 RequireNotNaN(Vector2 vector, string paramName)
        => VectorGuard.RequireNotNaN(vector, paramName);

    public static Vector2 RequireFinite(Vector2 vector, string paramName)
        => VectorGuard.RequireFinite(vector, paramName);

    public static Vector2 RequireNonZero(Vector2 vector, string paramName)
        => VectorGuard.RequireNonZero(vector, paramName);

    public static Vector2 RequireNormalized(Vector2 vector, string paramName)
        => VectorGuard.RequireNormalized(vector, paramName);

    public static Vector2 RequireValid(Vector2 vector, string paramName)
        => VectorGuard.RequireValid(vector, paramName);
}