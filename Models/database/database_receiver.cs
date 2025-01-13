namespace module_database;

using Microsoft.Data.SqlClient;

class DatabaseReceiver<T> : DatabaseMngr
    where T : DatabaseInfo, new()
{
    public static List<T> get_query_results(string query)
    {
        DatabaseReceiver<T> receiver = new DatabaseReceiver<T>();
        receiver.run(query);
        return receiver.m_results;
    }

    private List<T> m_results = new List<T>();

    protected override void row_itering(SqlDataReader reader)
    {
        T info = new T();
        info.fetch(reader);
        m_results.Add(info);
    }
}
