using Microsoft.Data.SqlClient;

sealed class IdCounterQuery
{
    // ========================================================================
    public enum State
    {
        none,
        id_hits_limit,
    }

    // ------------------------------------------------------------------------
    public static State s_last_state { get; private set; } = State.none;

    // ========================================================================
    public static bool get_count(SqlConnection conn, string table, out int id)
    {
        int count = 1;
        int max_count = 0;
        s_last_state = State.none;

        void func(SqlDataReader reader)
        {
            int pos = 0;
            count = DataReader.get_int(reader, ref pos);
            max_count = DataReader.get_int(reader, ref pos);
        }

        QueryCreator q = new(Tbl.id_counter);
        q.WhereClause(QPiece.eqStr(Fld.name, table));
        q.output(Fld.count, Fld.max_count);
        q.select(conn, func);

        if (count > max_count)
            s_last_state = State.id_hits_limit;

        if (max_count == 0)
            s_last_state = State.none;

        id = count;
        return s_last_state == State.none;
    }

    // ========================================================================
    public static bool increment(SqlConnection conn, string table, out int id)
    {
        if (!get_count(conn, table, out id))
            return false;

        QueryCreator q = new(Tbl.id_counter);
        q.SetClause(QPiece.eq(Fld.count, id + 1));
        q.WhereClause(QPiece.eq(Fld.name, table));
        q.update(conn);
        return true;
    }

    // ========================================================================
}

/* EOF */
