using System.Numerics;
namespace BattleStars;

public static class VectorValidator
{
    public static void ThrowIfNaN(Vector2 vector, string paramName)
    {
        FloatValidator.ThrowIfNaN(vector.X, $"{paramName}.X");
        FloatValidator.ThrowIfNaN(vector.Y, $"{paramName}.Y");
    }

    public static void ThrowIfInfinity(Vector2 vector, string paramName)
    {
        FloatValidator.ThrowIfInfinity(vector.X, $"{paramName}.X");
        FloatValidator.ThrowIfInfinity(vector.Y, $"{paramName}.Y");
    }

    public static void ThrowIfZero(Vector2 vector, string paramName)
    {
        if (vector == Vector2.Zero)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be a zero vector.");
    }

    public static void ThrowIfNotNormalized(Vector2 vector, string paramName)
    {
        // Allow slight floating-point imprecision
        if (Math.Abs(vector.LengthSquared() - 1f) > 0.001f)
            throw new ArgumentException($"{paramName} must be a normalized vector.", paramName);
    }
    
    public static void ThrowIfNaNOrInfinity(Vector2 vector, string paramName)
    {
        ThrowIfNaN(vector, paramName);
        ThrowIfInfinity(vector, paramName);
    }
}