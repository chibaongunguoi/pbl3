namespace module_data;

using Microsoft.Data.SqlClient;

sealed class DataFetcher<T> : DatabaseConnection
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> fetch(string query)
    {
        DataFetcher<T> receiver = new DataFetcher<T>();
        receiver.send(query);
        return receiver.results;
    }

    // ========================================================================
    private List<T> results;
    private string query;

    // ========================================================================
    DataFetcher()
    {
        this.results = new List<T>();
        this.query = "";
    }

    // ========================================================================
    protected override void action(SqlConnection connection)
    {
        this.results = DatabaseConnection.run_query<T>(connection, this.query);
    }

    // ========================================================================
    private void send(string query)
    {
        this.query = query;
        this.connect();
    }

    // ========================================================================
}

/* EOF */
