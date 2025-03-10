using Microsoft.Data.SqlClient;

class CommonQuery
{
    // ========================================================================
    public static void get_all_records(SqlConnection conn, Database.ReaderFunction f, Table table)
    {
        Query q = new(table);
        q.select(conn, f);
    }

    // ------------------------------------------------------------------------
    public static void get_record_by_id(
        Database.ReaderFunction f,
        int id,
        Table table,
        SqlConnection conn
    )
    {
        Query q = new(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        q.select(conn, f);
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

    // ========================================================================
    public static List<T> get_record_by_id(SqlConnection conn, int id, Table table)
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        return q.select<T>(conn);
    }

    // ========================================================================
    public static void insert_record(SqlConnection conn, T obj, Table table)
    {
        Query q = new(table);
        q.insert<T>(conn, obj);
    }

    // ========================================================================
}


/* EOF */
