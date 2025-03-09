sealed class SingleAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    private static Table s_latest_table;

    // ========================================================================
    public delegate List<T> TableFunction(Table table);

    // ========================================================================
    public static List<T> exec_table_function(TableFunction f)
    {
        List<T> result = new();
        foreach (var table in TableMngr.get_account_tables())
        {
            result = f(table);
            if (result.Count > 0)
            {
                s_latest_table = table;
                break;
            }
        }

        return result;
    }

    // ========================================================================
    public static List<T> get_account_by_id(int id) =>
        exec_table_function(table => CommonQuery<T>.get_record_by_id(table, id));

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username_password(string username, string password) =>
        exec_table_function(table =>
            CommonQuery<T>.get_account_by_username_password(table, username, password)
        );

    // ========================================================================
    public static Table get_latest_table() => s_latest_table;

    // ========================================================================
}

/* EOF */
