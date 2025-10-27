using BattleStars.Core.Guards.Utilities;
namespace BattleStars.Core.Guards;

public static class FloatGuard
{
    public static float RequireNotNaN(float value, string paramName)
    {
        if (float.IsNaN(value))
            throw new ArgumentException(ExceptionMessageFormatter.CannotBe(paramName, "NaN"), paramName);
        return value;
    }

    public static float RequireFinite(float value, string paramName)
    {
        if (float.IsInfinity(value))
            throw new ArgumentException(ExceptionMessageFormatter.MustBe(paramName, "finite"), paramName);
        return value;
    }

    public static float RequireNonNegative(float value, string paramName)
    {
        RequireValid(value, paramName);
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, ExceptionMessageFormatter.CannotBe(paramName, "negative"));
        return value;
    }

    public static float RequireNonZero(float value, string paramName)
    {
        RequireValid(value, paramName);
        if (value == 0)
            throw new ArgumentOutOfRangeException(paramName, ExceptionMessageFormatter.CannotBe(paramName, "zero"));
        return value;
    }

    public static float RequirePositive(float value, string paramName)
    {
        RequireValid(value, paramName);
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, ExceptionMessageFormatter.MustBe(paramName, "positive"));
        return value;
    }

    public static float RequireValid(float value, string paramName)
    {
        return RequireNotNaN(RequireFinite(value, paramName), paramName);
    }
}