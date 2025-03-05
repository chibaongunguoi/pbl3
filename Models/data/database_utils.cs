using Microsoft.Data.SqlClient;

class DatabaseUtils
{
    // ========================================================================
    public static string get_default_conn_string()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = ConfigOptionManager.get_server_name(),
            InitialCatalog = TableMngr.get_database_name(),
            IntegratedSecurity = true,
            TrustServerCertificate = true,
        };
        return builder.ConnectionString;
    }

    // ------------------------------------------------------------------------
    public static string get_server_only_conn_string()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = ConfigOptionManager.get_server_name(),
            IntegratedSecurity = true,
            TrustServerCertificate = true,
        };
        return builder.ConnectionString;
    }

    // ========================================================================
}

/* EOF */
