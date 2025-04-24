using Microsoft.Data.SqlClient;

sealed class IdCounterQuery
{
    // ========================================================================
    public enum Status
    {
        none,
        id_hits_limit,
    }

    // ------------------------------------------------------------------------
    public static Status s_last_status { get; private set; } = Status.none;

    // ========================================================================
    public static bool get_count(SqlConnection conn, string table, out int id)
    {
        int count = 1;
        int max_count = 0;
        s_last_status = Status.none;

        void func(SqlDataReader reader)
        {
            int pos = 0;
            count = QDataReader.GetInt(reader, ref pos);
            max_count = QDataReader.GetInt(reader, ref pos);
        }

        Query q = new(Tbl.id_counter);
        q.Where(Fld.name, table);
        q.OutputClause(Fld.count, Fld.max_count);
        q.Select(conn, func);

        if (count > max_count)
            s_last_status = Status.id_hits_limit;

        if (max_count == 0)
            s_last_status = Status.none;

        id = count;
        return s_last_status == Status.none;
    }

    // ========================================================================
    public static bool increment(SqlConnection conn, string table, out int id)
    {
        if (!get_count(conn, table, out id))
            return false;

        Query q = new(Tbl.id_counter);
        q.Where(Fld.name, table);
        q.Set(Fld.count, id + 1);
        q.Update(conn);
        return true;
    }

    // ========================================================================
}

/* EOF */
