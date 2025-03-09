class CommonQuery
{
    // ========================================================================
    public static void get_all_records(DatabaseConn.ReaderFunction f, Table table)
    {
        Query q = new(table);
        q.select(f);
    }

    // ------------------------------------------------------------------------
    public static void get_record_by_id(DatabaseConn.ReaderFunction f, int id, Table table)
    {
        Query q = new(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        q.select(f);
    }
    // ========================================================================
}

class CommonQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(Table table)
    {
        Query q = new(table);
        return q.select<T>();
    }

    // ========================================================================
    public static List<T> get_record_by_id(int id, Table table)
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        return q.select<T>();
    }

    // ========================================================================
}


/* EOF */
