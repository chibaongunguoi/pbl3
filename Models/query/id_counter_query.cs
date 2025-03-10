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
    public static int get_count(SqlConnection conn, Table table)
    {
        int count = 1;
        int max_count = 0;
        s_last_state = State.none;

        void func(SqlDataReader reader)
        {
            count = DataReader.get_int(reader, 0);
            max_count = DataReader.get_int(reader, 1);
        }

        Query q = new(Table.id_counter);
        q.where_(Field.id_counter__name, table.ToString());
        q.output(Field.id_counter__count);
        q.output(Field.id_counter__max_count);
        q.select(conn, func);

        if (count == max_count)
        {
            s_last_state = State.id_hits_limit;
        }

        return count;
    }

    // ========================================================================
    public static int increment(SqlConnection conn, Table table)
    {
        int new_id = get_count(conn, table);
        if (s_last_state == State.id_hits_limit)
        {
            return 0;
        }
        Query q = new(Table.id_counter);
        q.set_(Field.id_counter__count, new_id + 1);
        q.where_(Field.id_counter__name, table.ToString());
        q.update(conn);
        return new_id;
    }

    // ========================================================================
}

/* EOF */
