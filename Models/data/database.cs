using Microsoft.Data.SqlClient;

class Database
{
    // ========================================================================
    public static void run_query_new_conn(string query)
    {
        Database.call_func_new_conn(conn => Database.run_query_with_conn(conn, query));
    }

    // ========================================================================
    public static List<T> fetch_data_new_conn<T>(string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        Database.call_func_new_conn(conn =>
            results = Database.fetch_data_with_conn<T>(conn, query)
        );
        return results;
    }

    // ========================================================================
    public delegate void QueryFunction(SqlConnection conn);

    // ========================================================================
    private static string get_connection_string()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = Config.get_config("database", "server"),
            InitialCatalog = Config.get_config("database", "database"),
            IntegratedSecurity = true,
            TrustServerCertificate = true,
        };
        return builder.ConnectionString;
    }

    // ========================================================================
    public static void call_func_new_conn(QueryFunction f)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(Database.get_connection_string()))
            {
                conn.Open();
                f(conn);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // ========================================================================
    public static void run_query_with_conn(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    // ========================================================================
    public static List<T> fetch_data_with_conn<T>(SqlConnection conn, string query)
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
                    info.fetch_data_by_reader(reader, 0);
                    results.Add(info);
                }
            }
        }
        return results;
    }

    // ========================================================================
}

/* EOF */
