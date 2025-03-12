using System.Text.RegularExpressions;

class RegexPatterns
{
    // ========================================================================
    public static string username { get; } = "^[a-zA-Z0-9_]{3,16}$";
    public static string password { get; } =
        "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$";
    public static string tel { get; } = @"^\d{10-11}$";
    public static string numeric { get; } = @"^\d+$";

    // ========================================================================
    public static bool match(string str, string pattern)
    {
        return Regex.IsMatch(str, pattern);
    }
    // ========================================================================
}

/* EOF */
