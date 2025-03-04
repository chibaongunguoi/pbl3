using Microsoft.Data.SqlClient;

class Database
{
    // ========================================================================
    public delegate void ConnFunction(SqlConnection conn);

    // ========================================================================
    public static void exec(ConnFunction f, string? conn_string = null)
    {
        conn_string = conn_string == null ? DatabaseUtils.get_default_conn_string() : conn_string;

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
    public static void exec_non_query(string query, string? conn_string = null)
    {
        Database.exec(conn => DatabaseConn.exec_non_query(conn, query), conn_string);
    }

    // ------------------------------------------------------------------------
    public static List<T> exec_query<T>(string query, string? conn_string = null)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        Database.exec(conn => results = DatabaseConn.exec_query<T>(conn, query), conn_string);
        return results;
    }

    // ------------------------------------------------------------------------
    public static List<string> exec_query(string query, string? conn_string = null)
    {
        List<string> results = new List<string>();
        Database.exec(conn => results = DatabaseConn.exec_query(conn, query), conn_string);
        return results;
    }

    // ------------------------------------------------------------------------
    public static void exec_function(
        string query,
        DatabaseConn.ReaderFunction f,
        string? conn_string = null
    )
    {
        Database.exec(conn => DatabaseConn.exec_function(conn, query, f), conn_string);
    }

    // ========================================================================
}

/* EOF */
