class MultiAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(Table table);

    // ========================================================================
    public static List<T> exec_function(QueryFunction f)
    {
        List<T> result = new List<T>();
        foreach (var table in TableMngr.get_account_tables())
        {
            result.AddRange(f(table));
        }

        return result;
    }

    // ========================================================================
    public List<T> get_all_accounts()
    {
        return exec_function(table => CommonQuery<T>.get_all_records(table));
    }

    // ========================================================================
}

/* EOF */
