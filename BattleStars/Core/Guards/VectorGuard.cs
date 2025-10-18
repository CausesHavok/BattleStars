using System.Numerics;
namespace BattleStars.Core.Guards;

public static class VectorGuard
{
    public static void RequireNotNaN(Vector2 vector, string paramName)
    {
        FloatGuard.RequireNotNaN(vector.X, $"{paramName}.X");
        FloatGuard.RequireNotNaN(vector.Y, $"{paramName}.Y");
    }

    public static void RequireFinite(Vector2 vector, string paramName)
    {
        FloatGuard.RequireFinite(vector.X, $"{paramName}.X");
        FloatGuard.RequireFinite(vector.Y, $"{paramName}.Y");
    }

    public static void RequireNonZero(Vector2 vector, string paramName)
    {
        if (vector == Vector2.Zero)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be a zero vector.");
    }

    public static void RequireNormalized(Vector2 vector, string paramName)
    {
        // Allow slight floating-point imprecision
        if (Math.Abs(vector.LengthSquared() - 1f) > 0.001f)
            throw new ArgumentException($"{paramName} must be a normalized vector.", paramName);
    }
    
    public static void RequireValid(Vector2 vector, string paramName)
    {
        RequireNotNaN(vector, paramName);
        RequireFinite(vector, paramName);
    }
}