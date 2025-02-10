namespace module_data;

using Microsoft.Data.SqlClient;
using module_config;

class Database
{
    // ========================================================================
    public static string get_connection_string()
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
    public static void execute_query(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    // ========================================================================
    public static List<T> fetch_data_by_query<T>(SqlConnection connection, string query)
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
