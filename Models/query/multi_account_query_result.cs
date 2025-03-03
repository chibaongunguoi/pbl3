class MultiAccountQueryResult<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(string table_name);

    // ========================================================================
    public List<T> result = new List<T>();

    // ========================================================================
    public void exec_function(QueryFunction f)
    {
        result.Clear();
        foreach (var kv in ConfigUtils.get_table_name_dict())
        {
            result.AddRange(f(kv.Value));
        }
    }

    // ========================================================================
}

/* EOF */
