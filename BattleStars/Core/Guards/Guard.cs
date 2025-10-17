namespace BattleStars.Core.Guards;

public static class Guard
{
    public static T NotNull<T>(this T? value, string name)
      where T : class
      => value ?? throw new ArgumentNullException(name);
}