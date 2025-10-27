namespace BattleStars.Core.Guards.Utilities;
public static class ExceptionMessageFormatter
{
    public static string CannotBe(string paramName, string condition) =>
        $"{paramName} cannot be {condition}.";

    public static string MustBe(string paramName, string requirement) =>
        $"{paramName} must be {requirement}.";

    public static string IsInvalid(string paramName) =>
        $"{paramName} is invalid.";

    public static string MustBeLessThan(string paramName1, string paramName2) =>
        $"{paramName1} must be less than {paramName2}.";

    public static string MustBeNormalized(string paramName) =>
        $"{paramName} must be a normalized vector.";

    public static string CannotBeZeroVector(string paramName) =>
        $"{paramName} cannot be a zero vector.";

    public static string InvalidEnumValue(Type enumType) =>
        $"Invalid value for enum type {enumType.Name}.";

    public static string CannotBeEmpty(string paramName) =>
        $"{paramName} cannot be empty.";
}