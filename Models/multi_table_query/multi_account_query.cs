class MultiAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(string table_name);

    // ========================================================================
    public static List<T> exec_function(QueryFunction f)
    {
        List<T> result = new List<T>();
        foreach (var table in DatabaseConfigManager.get_account_tables())
        {
            string table_name = DatabaseConfigManager.get_table_name(table);
            result.AddRange(f(table_name));
        }

        return result;
    }

    // ========================================================================
    public List<T> get_all_accounts()
    {
        return exec_function(table_name => RecordQueryFromTable<T>.get_all_records(table_name));
    }

    // ========================================================================
}

/* EOF */
