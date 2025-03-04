sealed class SingleAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    private static InfoAccountType s_latest_account_type;

    // ========================================================================
    public delegate List<T> QueryFunction(string table_name);

    // ========================================================================
    public static List<T> exec_function(QueryFunction f)
    {
        List<T> result = new();
        foreach (var (key, table_config) in DatabaseConfigManager.get_account_table_config_dict())
        {
            result = f(table_config.name);
            if (result.Count > 0)
            {
                s_latest_account_type = key;
                break;
            }
        }

        return result;
    }

    // ========================================================================
    public static List<T> get_account_by_id(int id) =>
        exec_function(table_name => RecordQueryFromTable<T>.get_record_by_id(table_name, id));

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username_password(string username, string password) =>
        exec_function(table_name =>
            AccountQueryFromTable<T>.get_account_by_username_password(
                table_name,
                username,
                password
            )
        );

    // ========================================================================
    public static InfoAccountType get_latest_account_type() => s_latest_account_type;

    // ========================================================================
}

/* EOF */
