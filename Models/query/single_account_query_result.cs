class SingleAccountQueryResult<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(string table_name);

    // ========================================================================
    public InfoAccountType account_type;
    public List<T> result = new List<T>();

    // ========================================================================
    public void exec_function(QueryFunction f)
    {
        result.Clear();
        foreach (var kv in ConfigUtils.get_table_name_dict())
        {
            result = f(kv.Value);
            if (result.Count > 0)
            {
                account_type = kv.Key;
                break;
            }
        }
    }

    // ========================================================================
}

/* EOF */
