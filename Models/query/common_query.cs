using Microsoft.Data.SqlClient;

class CommonQuery
{
    // ------------------------------------------------------------------------
    public static void insert_record_with_id<T>(SqlConnection conn, T obj, Table table)
        where T : IdObj, new()
    {
        // int next_id = IdCounterQuery.increment(conn, table);
        // if (IdCounterQuery.s_last_state == IdCounterQuery.State.id_hits_limit)
        //     return;
        //
        // obj.id = next_id;
        //
        // Query q = new(table);
        // q.insert<T>(conn, obj);
    }

    // ========================================================================
}

class CommonQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(SqlConnection conn, string table)
    {
        QueryCreator q = new(table);
        return q.select<T>(conn);
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(SqlConnection conn, string table, int id)
    {
        QueryCreator q = new(table);
        q.Where(table, Fld.id, id);
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
