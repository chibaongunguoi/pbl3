namespace module_data;

using Microsoft.Data.SqlClient;

sealed class DataFetcher<T> : QuerySender
    where T : DataObj, new()
{
    public static List<T> fetch(string query)
    {
        DataFetcher<T> receiver = new DataFetcher<T>();
        receiver._send(query);
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
