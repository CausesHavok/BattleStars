namespace BattleStars.Utility;

public static class FloatValidator
{
    public static void ThrowIfNaN(float value, string paramName)
    {
        if (float.IsNaN(value))
            throw new ArgumentException($"{paramName} cannot be NaN.", paramName);
    }

    public static void ThrowIfInfinity(float value, string paramName)
    {
        if (float.IsInfinity(value))
            throw new ArgumentException($"{paramName} cannot be Infinity.", paramName);
    }

    public static void ThrowIfNegative(float value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be negative.");
    }

    public static void ThrowIfZero(float value, string paramName)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} cannot be zero.");
    }

    public static void ThrowIfNegativeOrZero(float value, string paramName)
    {
        ThrowIfNegative(value, paramName);
        ThrowIfZero(value, paramName);
    }

    public static void ThrowIfNaNOrInfinity(float value, string paramName)
    {
        ThrowIfNaN(value, paramName);
        ThrowIfInfinity(value, paramName);
    }

}