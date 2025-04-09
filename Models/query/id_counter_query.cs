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
    public static bool get_count(SqlConnection conn, Table table, out int id)
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

        Query q = new(Table.id_counter);
        q.where_(Field.id_counter__name, table.ToString());
        q.output(Field.id_counter__count);
        q.output(Field.id_counter__max_count);
        q.select(conn, func);

        if (count > max_count)
            s_last_state = State.id_hits_limit;

        if (max_count == 0)
            s_last_state = State.none;

        id = count;
        return s_last_state == State.none;
    }

    // ========================================================================
    public static bool increment(SqlConnection conn, Table table, out int id)
    {
        if (!get_count(conn, table, out id))
            return false;

        Query q = new(Table.id_counter);
        q.set_(Field.id_counter__count, id + 1);
        q.where_(Field.id_counter__name, table.ToString());
        q.update(conn);
        return true;
    }

    // ========================================================================
}

/* EOF */
