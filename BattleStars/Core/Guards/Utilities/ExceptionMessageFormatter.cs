namespace BattleStars.Core.Guards.Utilities;
public static class ExceptionMessageFormatter
{
    public static string CannotBe(string paramName, string condition) =>
        $"{paramName} cannot be {condition}.";

    public static string MustBe(string paramName, string requirement) =>
        $"{paramName} must be {requirement}.";

    public static string IsInvalid(string paramName) =>
        $"{paramName} is invalid.";
}
