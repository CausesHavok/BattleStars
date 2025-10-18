namespace BattleStars.Core.Guards;

public static class FloatGuard
{

    public static void RequireNotNaN(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (float.IsNaN(value))
            throw new ArgumentException($"{paramName} cannot be NaN.", paramName);
    }

    public static void RequireFinite(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (float.IsInfinity(value))
            throw new ArgumentException($"{paramName} cannot be Infinity.", paramName);
    }

    public static void RequireNonNegative(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be negative.");
    }

    public static void RequireNonZero(float value, string paramName)
    {
        Guard.NotNull(paramName, nameof(paramName));
        if (value == 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be zero.");
    }

    public static void RequirePositive(float value, string paramName)
    {
        RequireNonNegative(value, paramName);
        RequireNonZero(value, paramName);
    }

    public static void RequireValid(float value, string paramName)
    {
        RequireNotNaN(value, paramName);
        RequireFinite(value, paramName);
    }

}