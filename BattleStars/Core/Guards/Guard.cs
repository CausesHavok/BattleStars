using System.Numerics;

namespace BattleStars.Core.Guards;

public static class Guard
{
    public static T NotNull<T>(this T? value, string name) where T : class
        => NullGuard.NotNull(value, name);

    public static void RequireNotNaN(float value, string paramName)
        => FloatGuard.RequireNotNaN(value, paramName);

    public static void RequireFinite(float value, string paramName)
        => FloatGuard.RequireFinite(value, paramName);

    public static void RequireNonNegative(float value, string paramName)
        => FloatGuard.RequireNonNegative(value, paramName);

    public static void RequireNonZero(float value, string paramName)
        => FloatGuard.RequireNonZero(value, paramName);

    public static void RequirePositive(float value, string paramName)
        => FloatGuard.RequirePositive(value, paramName);

    public static void RequireValid(float value, string paramName)
        => FloatGuard.RequireValid(value, paramName);

    public static void RequireNotNaN(Vector2 vector, string paramName)
        => VectorGuard.RequireNotNaN(vector, paramName);

    public static void RequireFinite(Vector2 vector, string paramName)
        => VectorGuard.RequireFinite(vector, paramName);
    
    public static void RequireNonZero(Vector2 vector, string paramName)
        => VectorGuard.RequireNonZero(vector, paramName);

    public static void RequireNormalized(Vector2 vector, string paramName)
        => VectorGuard.RequireNormalized(vector, paramName);

    public static void RequireValid(Vector2 vector, string paramName)
        => VectorGuard.RequireValid(vector, paramName);
}