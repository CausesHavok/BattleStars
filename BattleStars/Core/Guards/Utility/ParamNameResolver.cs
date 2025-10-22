namespace BattleStars.Core.Guards.Utility
{
    internal static class ParamNameResolver
    {
        public static string Resolve(string? paramName)
            => string.IsNullOrWhiteSpace(paramName) ? "<value>" : paramName;
    }
}