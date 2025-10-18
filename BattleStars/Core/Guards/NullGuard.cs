namespace BattleStars.Core.Guards;

public static class NullGuard
{
    public static T NotNull<T>(T? value, string paramName) where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }
}