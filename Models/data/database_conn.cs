using Microsoft.Data.SqlClient;

class DatabaseConn
{
    // ========================================================================
    public static void exec_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            command.ExecuteNonQuery();
        }
    }

    // ========================================================================
    public static List<T> fetch_data<T>(SqlConnection conn, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T info = new T();
                    info.fetch_data(reader, 0);
                    results.Add(info);
                }
            }
        }
        return results;
    }

    // ========================================================================
}

/* EOF */
