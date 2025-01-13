namespace module_data;

using Microsoft.Data.SqlClient;

sealed class QuerySender : DatabaseConnection
{
    // ========================================================================
    public static void run(string query)
    {
        QuerySender sender = new QuerySender();
        sender.send(query);
    }

    // ========================================================================
    private string query;

    // ========================================================================
    public QuerySender()
    {
        this.query = "";
    }

    // ========================================================================
    protected override void action(SqlConnection connection)
    {
        DatabaseConnection.run_query(connection, this.query);
    }

    // ========================================================================
    private void send(string query)
    {
        this.query = query;
        connect();
    }

    // ========================================================================
}

/* EOF */
