class QueryUtils
{
    // ========================================================================
    // INFO: Định dạng bao bọc tên trong SQL, ví dụ "CREATE DATABSE" [database_name]
    public static string get_bracket_format(string s) => "[" + s + "]";

    // ------------------------------------------------------------------------

    // INFO: Định dạng bao bọc tên trong SQL, ví dụ "SELECT [table_name].[field_name]"
    public static string get_bracket_format(string table_name, string field) =>
        "[" + table_name + "].[" + field + "]";

    // ========================================================================
}

/* EOF */
