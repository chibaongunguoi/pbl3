namespace module_data;

using Microsoft.Data.SqlClient;

class DatabaseConnection
{
    // ========================================================================
    protected static void run_query(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    // ------------------------------------------------------------------------
    protected static List<T> run_query<T>(SqlConnection connection, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T info = new T();
                    info.fetch(reader, 0);
                    results.Add(info);
                }
            }
        }
        return results;
    }

    // ========================================================================
    protected void connect()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Database.get_connection_string()))
            {
                connection.Open();
                this.action(connection);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // ========================================================================
    protected virtual void action(SqlConnection connection) { }

    // ========================================================================
}

/* EOF */
