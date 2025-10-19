namespace BattleStars.Core.Guards;

public static class FloatGuard
{

    public static float RequireNotNaN(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (float.IsNaN(value))
            throw new ArgumentException($"{paramName} cannot be NaN.", paramName);
        return value;
    }

    public static float RequireFinite(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (float.IsInfinity(value))
            throw new ArgumentException($"{paramName} cannot be Infinity.", paramName);
        return value;
    }

    public static float RequireNonNegative(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be negative.");
        return value;
    }

    public static float RequireNonZero(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (value == 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be zero.");
        return value;
    }

    public static float RequirePositive(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be positive.");
        return value;
    }

    public static float RequireValid(float value, string paramName)
    {
        return RequireNotNaN(RequireFinite(value, paramName), paramName);
    }
}