namespace module_data;

using Microsoft.Data.SqlClient;

sealed class QueryExecutor : DatabaseConnection
{
    // ========================================================================
    protected override void process_connection(SqlConnection connection)
    {
        Database.execute_query(connection, this.query);
    }

    // ========================================================================
    private string query;

    // ========================================================================
    public QueryExecutor()
    {
        this.query = "";
    }

    // ========================================================================
    private void execute_query(string query)
    {
        this.query = query;
        run_kernel();
    }

    // ========================================================================
    public static void run_query(string query)
    {
        QueryExecutor executor = new QueryExecutor();
        executor.execute_query(query);
    }

    // ========================================================================
}

/* EOF */
