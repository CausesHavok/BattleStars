using System.Numerics;
using System.Runtime.CompilerServices;
using BattleStars.Core.Guards.Utilities;

namespace BattleStars.Core.Guards;

public static class Guard
{
    public static T NotNull<T>(this T? value, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : class
        => NullGuard.NotNull(value, ParamNameResolver.Resolve(paramName));

    public static float RequireNotNaN(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequireNotNaN(value, ParamNameResolver.Resolve(paramName));

    public static float RequireFinite(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequireFinite(value, ParamNameResolver.Resolve(paramName));

    public static float RequireNonNegative(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequireNonNegative(value, ParamNameResolver.Resolve(paramName));

    public static float RequireNonZero(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequireNonZero(value, ParamNameResolver.Resolve(paramName));

    public static float RequirePositive(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequirePositive(value, ParamNameResolver.Resolve(paramName));

    public static float RequireValid(float value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        => FloatGuard.RequireValid(value, ParamNameResolver.Resolve(paramName));

    public static Vector2 RequireNotNaN(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
        => VectorGuard.RequireNotNaN(vector, ParamNameResolver.Resolve(paramName));

    public static Vector2 RequireFinite(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
        => VectorGuard.RequireFinite(vector, ParamNameResolver.Resolve(paramName));

    public static Vector2 RequireNonZero(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
        => VectorGuard.RequireNonZero(vector, ParamNameResolver.Resolve(paramName));

    public static Vector2 RequireNormalized(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
        => VectorGuard.RequireNormalized(vector, ParamNameResolver.Resolve(paramName));

    public static Vector2 RequireValid(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
        => VectorGuard.RequireValid(vector, ParamNameResolver.Resolve(paramName));

    public static IEnumerable<T> RequireNotEmpty<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? paramName = null)
        => CollectionGuard.RequireNotEmpty(collection, ParamNameResolver.Resolve(paramName));
}