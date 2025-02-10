namespace module_data;

using Microsoft.Data.SqlClient;

sealed class DataFetcher<T> : DatabaseConnection
    where T : DataObj, new()
{
    // ========================================================================
    protected override void process_connection(SqlConnection connection)
    {
        this.results = Database.fetch_data_by_query<T>(connection, this.query);
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
    private List<T> process_query(string query)
    {
        this.query = query;
        this.run_kernel();
        return this.results;
    }

    // ========================================================================
    public static List<T> fetch_data_by_query(string query)
    {
        DataFetcher<T> receiver = new DataFetcher<T>();
        return receiver.process_query(query);
    }

    // ========================================================================
}

/* EOF */
