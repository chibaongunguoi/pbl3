using System.Text.RegularExpressions;

class RegexPatterns
{
    // ========================================================================
    public const string username = "^[a-zA-Z0-9_]{3,16}$";
    public const string password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$";
    public const string tel = @"^\d{10-11}$";
    public const string numeric = @"^\d+$";

    // ========================================================================
    public static bool match(string str, string pattern)
    {
        return Regex.IsMatch(str, pattern);
    }
    // ========================================================================
}

/* EOF */
