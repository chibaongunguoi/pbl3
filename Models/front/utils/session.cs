public static class UrlKey
{
    public const string tchId = "tchId",
        courseId = "courseId",
        page = "page";
}

public static class SessionKey
{
    public const string TRUE = "TRUE",
        FALSE = "FALSE";
}

public static class SessionForm
{
    public static Dictionary<string, string> errors = new();
    public static bool displaying_error = false;

    public static bool contains(string key)
    {
        return errors.ContainsKey(key);
    }
}

public static class Session
{
    public static int? getInt(IQueryCollection query, string key)
    {
        string? value = query[key];
        if (value is null)
            return null;
        int result = 0;
        if (int.TryParse(value, out result))
            return result;
        else
            return null;
    }

    public static string? getStr(IQueryCollection query, string key)
    {
        string? value = query[key];
        return value;
    }
}
