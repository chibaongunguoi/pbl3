using Microsoft.Data.SqlClient;

class CommonQuery
{
    // ========================================================================
    public static List<string> get_all_records(SqlConnection conn, Table table)
    {
        Query q = new(table);
        return q.select(conn);
    }

    // ------------------------------------------------------------------------
    public static List<string> get_record_by_id(int id, Table table, SqlConnection conn)
    {
        Query q = new(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        return q.select(conn);
    }

    // ------------------------------------------------------------------------
    public static void insert_record_with_id<T>(SqlConnection conn, T obj, Table table)
        where T : IdObj, new()
    {
        int next_id = 0;
        next_id = IdCounterQuery.increment(conn, table);
        if (IdCounterQuery.s_last_state == IdCounterQuery.State.id_hits_limit)
        {
            return;
        }

        obj.id = next_id;

        Query q = new(table);
        q.insert<T>(conn, obj);
    }

    // ========================================================================
}

class CommonQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(SqlConnection conn, Table table)
    {
        Query q = new(table);
        return q.select<T>(conn);
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(SqlConnection conn, int id, Table table)
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        return q.select<T>(conn);
    }

    // ------------------------------------------------------------------------
    public static void insert_record(SqlConnection conn, T obj, Table table)
    {
        Query q = new(table);
        q.insert<T>(conn, obj);
    }

    // ========================================================================
}


/* EOF */
