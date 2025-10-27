using BattleStars.Core.Guards.Utilities;

namespace BattleStars.Core.Guards;

public static class CollectionGuard
{
    public static IEnumerable<T> RequireNotEmpty<T>(IEnumerable<T> collection, string paramName)
    {
        if (!collection.Any())
            throw new ArgumentException(ExceptionMessageFormatter.CannotBeEmpty(paramName), paramName);
        return collection;
    }
}