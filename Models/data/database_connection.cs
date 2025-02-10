namespace module_data;

using Microsoft.Data.SqlClient;

class DatabaseConnection
{
    // ========================================================================
    protected virtual void process_connection(SqlConnection connection) { }

    // ========================================================================
    protected void run_kernel()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Database.get_connection_string()))
            {
                connection.Open();
                this.process_connection(connection);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // ========================================================================
}

/* EOF */
