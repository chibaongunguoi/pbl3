using Microsoft.Data.SqlClient;

class CommonQuery
{
    // ========================================================================
}

class CommonQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(SqlConnection conn, string table)
    {
        Query q = new(table);
        return q.select<T>(conn);
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(SqlConnection conn, string table, int id)
    {
        Query q = new(table);
        q.Where(table, Fld.id, id);
        return q.select<T>(conn);
    }

    // ------------------------------------------------------------------------
    public static void insert_record(SqlConnection conn, T obj, string table)
    {
        Query q = new(table);
        q.insert<T>(conn, obj);
    }

    // ========================================================================
}


/* EOF */
