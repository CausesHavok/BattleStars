using System.Numerics;
using System.Runtime.CompilerServices;
using BattleStars.Core.Guards.Utility;
namespace BattleStars.Core.Guards;

public static class VectorGuard
{
    public static Vector2 RequireNotNaN(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
    {
        var resolvedParamName = ParamNameResolver.Resolve(paramName);
        Guard.RequireNotNaN(vector.X, $"{resolvedParamName}.X");
        Guard.RequireNotNaN(vector.Y, $"{resolvedParamName}.Y");
        return vector;
    }

    public static Vector2 RequireFinite(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
    {
        var resolvedParamName = ParamNameResolver.Resolve(paramName);
        Guard.RequireFinite(vector.X, $"{resolvedParamName}.X");
        Guard.RequireFinite(vector.Y, $"{resolvedParamName}.Y");
        return vector;
    }

    public static Vector2 RequireNonZero(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
    {
        if (vector == Vector2.Zero)
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentOutOfRangeException(resolvedParamName, $"{resolvedParamName} cannot be a zero vector.");
        }
        return vector;
    }

    public static Vector2 RequireNormalized(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
    {
        // Allow slight floating-point imprecision
        if (Math.Abs(vector.LengthSquared() - 1f) > 0.001f)
        {
            var resolvedParamName = ParamNameResolver.Resolve(paramName);
            throw new ArgumentException($"{resolvedParamName} must be a normalized vector.", resolvedParamName);
        }
        return vector;
    }

    public static Vector2 RequireValid(Vector2 vector, [CallerArgumentExpression(nameof(vector))] string? paramName = null)
    {
        return RequireFinite(RequireNotNaN(vector, paramName), paramName);
    }
}