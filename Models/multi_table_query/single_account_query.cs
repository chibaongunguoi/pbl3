sealed class SingleAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    private static Table s_latest_table;

    // ========================================================================
    public delegate List<T> TableFunction(string table_name);

    // ========================================================================
    public static List<T> exec_table_function(TableFunction f)
    {
        List<T> result = new();
        foreach (var table in DatabaseConfigManager.get_account_tables())
        {
            string table_name = DatabaseConfigManager.get_table_name(table);
            result = f(table_name);
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
        exec_table_function(table_name => RecordQueryFromTable<T>.get_record_by_id(table_name, id));

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username_password(string username, string password) =>
        exec_table_function(table_name =>
            AccountQueryFromTable<T>.get_account_by_username_password(
                table_name,
                username,
                password
            )
        );

    // ========================================================================
    public static Table get_latest_table() => s_latest_table;

    // ========================================================================
}

/* EOF */
