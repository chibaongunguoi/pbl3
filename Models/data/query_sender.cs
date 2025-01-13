namespace module_data;

using Microsoft.Data.SqlClient;
using module_config;

class QuerySender
{
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

    public static void send(string query)
    {
        QuerySender sender = new QuerySender();
        sender._send(query);
    }

    protected void _send(string query)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(get_connection_string()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader data_reader = command.ExecuteReader())
                    {
                        while (data_reader.Read())
                        {
                            this.row_itering(data_reader);
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    protected virtual void row_itering(SqlDataReader reader) { }
}
