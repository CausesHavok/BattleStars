using System.Numerics;
namespace BattleStars.Core.Guards;

public static class VectorGuard
{
    public static Vector2 RequireNotNaN(Vector2 vector, string paramName)
    {
        Guard.RequireNotNaN(vector.X, $"{paramName}.X");
        Guard.RequireNotNaN(vector.Y, $"{paramName}.Y");
        return vector;
    }

    public static Vector2 RequireFinite(Vector2 vector, string paramName)
    {
        Guard.RequireFinite(vector.X, $"{paramName}.X");
        Guard.RequireFinite(vector.Y, $"{paramName}.Y");
        return vector;
    }

    public static Vector2 RequireNonZero(Vector2 vector, string paramName)
    {
        if (vector == Vector2.Zero)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be a zero vector.");
        return vector;
    }

    public static Vector2 RequireNormalized(Vector2 vector, string paramName)
    {
        // Allow slight floating-point imprecision
        if (Math.Abs(vector.LengthSquared() - 1f) > 0.001f)
            throw new ArgumentException($"{paramName} must be a normalized vector.", paramName);
        return vector;
    }

    public static Vector2 RequireValid(Vector2 vector, string paramName)
    {
        return RequireFinite(RequireNotNaN(vector, paramName), paramName);
    }
}