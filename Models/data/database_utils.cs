using Microsoft.Data.SqlClient;

class DatabaseUtils
{
    // ========================================================================
    // INFO: Trả về xâu kết nối mặc định tới cơ sở dữ liệu.
    public static string get_default_conn_string()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = ConfigOptionManager.get_server_name(),
            InitialCatalog = TableMngr.get_database_name(),
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            ConnectTimeout = 60,
            MultipleActiveResultSets = true,
        };
        return builder.ConnectionString;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về xâu kết nối chỉ tới server.
    public static string get_server_only_conn_string()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = ConfigOptionManager.get_server_name(),
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            ConnectTimeout = 60,
        };
        return builder.ConnectionString;
    }

    // ========================================================================
}

/* EOF */
