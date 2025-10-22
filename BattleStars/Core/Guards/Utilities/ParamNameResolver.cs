namespace BattleStars.Core.Guards.Utilities
{
    internal static class ParamNameResolver
    {
        public static string Resolve(string? paramName)
            => string.IsNullOrWhiteSpace(paramName) ? "<value>" : paramName;
    }
}