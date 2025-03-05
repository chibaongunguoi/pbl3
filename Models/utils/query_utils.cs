class QueryUtils
{
    // ========================================================================
    public static string get_bracket_format(string s) => "[" + s + "]";

    // ------------------------------------------------------------------------

    public static string get_bracket_format(string table_name, string field) =>
        "[" + table_name + "].[" + field + "]";

    // ========================================================================
}

/* EOF */
