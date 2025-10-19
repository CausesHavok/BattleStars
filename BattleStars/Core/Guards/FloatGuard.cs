using System.Runtime.CompilerServices;
using BattleStars.Core.Guards.Utilities;
namespace BattleStars.Core.Guards;

public static class FloatGuard
{
    public static float RequireNotNaN(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (float.IsNaN(value))
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentException($"{resolvedParamName} cannot be NaN.", resolvedParamName);
        }
        return value;
    }

    public static float RequireFinite(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (float.IsInfinity(value))
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentException($"{resolvedParamName} must be finite.", resolvedParamName);
        }
        return value;
    }

    public static float RequireNonNegative(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentOutOfRangeException(resolvedParamName, $"{resolvedParamName} cannot be negative.");
        }
        return value;
    }

    public static float RequireNonZero(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == 0)
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentOutOfRangeException(resolvedParamName, $"{resolvedParamName} cannot be zero.");
        }
        return value;
    }

    public static float RequirePositive(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentOutOfRangeException(resolvedParamName, $"{resolvedParamName} must be positive.");
        }
        return value;
    }

    public static float RequireValid(
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        return RequireNotNaN(RequireFinite(value, paramName), paramName);
    }
}