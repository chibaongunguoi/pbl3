using Microsoft.Data.SqlClient;

class Database
{
    // ========================================================================
    public static void exec_query(string query)
    {
        Database.exec_function(conn => DatabaseConn.exec_query(conn, query));
    }

    // ========================================================================
    public static List<T> exec_query<T>(string query, string conn_string = "")
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        Database.exec_function(
            conn => results = DatabaseConn.fetch_data<T>(conn, query),
            conn_string
        );
        return results;
    }

    // ========================================================================
    public delegate void QueryFunction(SqlConnection conn);

    // ========================================================================
    public static void exec_function(QueryFunction f, string conn_string = "")
    {
        conn_string = conn_string == "" ? DatabaseUtils.get_default_conn_string() : conn_string;

        try
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                f(conn);
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
