namespace module_regex;

using System.Text.RegularExpressions;

class RegexPatterns
{
    // ========================================================================
    public static string username = "^[a-zA-Z0-9_]{3,16}$";
    public static string password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$";
    public static string tel = @"^\d{10-11}$";

    // ========================================================================
    public static bool match(string str, string pattern)
    {
        return Regex.IsMatch(str, pattern);
    }
    // ========================================================================
}

/* EOF */
