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
}

/* EOF */
