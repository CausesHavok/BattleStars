namespace BattleStars.Core.Guards;

public static class FloatGuard
{
    private static void RequireNotNull(object value, string paramName)
    {
        Guard.NotNull(value, paramName);
    }

    public static void RequireNotNaN(float value, string paramName)
    {
        RequireNotNull(paramName, nameof(paramName));
        if (float.IsNaN(value))
            throw new ArgumentException($"{paramName} cannot be NaN.", paramName);
    }

    public static void RequireFinite(float value, string paramName)
    {
        RequireNotNull(paramName, nameof(paramName));
        if (float.IsInfinity(value))
            throw new ArgumentException($"{paramName} cannot be Infinity.", paramName);
    }

    public static void RequireNonNegative(float value, string paramName)
    {
        RequireNotNull(paramName, nameof(paramName));
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be negative.");
    }

    public static void RequireNonZero(float value, string paramName)
    {
        RequireNotNull(paramName, nameof(paramName));
        if (value == 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be zero.");
    }

    public static void RequirePositive(float value, string paramName)
    {
        RequireNonNegative(value, paramName);
        RequireNonZero(value, paramName);
    }

    public static void RequireValidFloat(float value, string paramName)
    {
        RequireNotNaN(value, paramName);
        RequireFinite(value, paramName);
    }

}