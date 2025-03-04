using Microsoft.Data.SqlClient;

class DatabaseConn
{
    // ========================================================================
    public delegate void ReaderFunction(SqlConnection conn, SqlDataReader reader);

    // ========================================================================
    public static void exec_non_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            command.ExecuteNonQuery();
        }
    }

    // ========================================================================
    public static void read<T>(SqlConnection conn, SqlDataReader reader, ref List<T> result)
        where T : DataObj, new()
    {
        T info = new T();
        info.fetch_data(reader, 0);
        result.Add(info);
    }

    // ------------------------------------------------------------------------
    public static void read(SqlConnection conn, SqlDataReader reader, ref List<string> result)
    {
        List<string> record = new();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            record.Add(reader[i].ToString() ?? "");
        }

        result.Add(string.Join(",", record));
    }

    // ------------------------------------------------------------------------
    public static List<T> exec_query<T>(SqlConnection conn, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        exec_function(conn, query, (c, r) => read(c, r, ref results));
        return results;
    }

    // ------------------------------------------------------------------------
    public static List<string> exec_query(SqlConnection conn, string query)
    {
        List<string> results = new List<string>();
        exec_function(conn, query, (c, r) => read(c, r, ref results));
        return results;
    }

    // ========================================================================
    public static void exec_function(SqlConnection conn, string query, ReaderFunction f)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    f(conn, reader);
                }
            }
        }
    }

    // ========================================================================
}

/* EOF */
