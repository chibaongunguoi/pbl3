using Microsoft.Data.SqlClient;

class Database
{
    // ========================================================================
    public delegate void ConnFunction(SqlConnection conn);

    // ========================================================================
    // INFO: Tạo mới conn và truyền conn vào ConnFunction.
    public static void exec_conn_function(ConnFunction conn_function, string? conn_string = null)
    {
        conn_string = conn_string == null ? DatabaseUtils.get_default_conn_string() : conn_string;

        try
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                conn_function(conn);
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    // ========================================================================
    // INFO: Chạy non_query.
    public static void exec_non_query(string query, string? conn_string = null)
    {
        Database.exec_conn_function(conn => DatabaseConn.exec_non_query(conn, query), conn_string);
    }

    // ========================================================================
    // INFO: Trả về các DataObj thu được từ query.
    public static List<T> exec_query<T>(string query, string? conn_string = null)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        Database.exec_conn_function(
            conn => results = DatabaseConn.exec_query<T>(conn, query),
            conn_string
        );
        return results;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về biểu diễn dạng string của các bản ghi thu được từ query.
    public static List<string> exec_query(string query, string? conn_string = null)
    {
        List<string> results = new List<string>();
        ConnFunction conn_function = (conn) => results = DatabaseConn.exec_query(conn, query);
        Database.exec_conn_function(conn_function, conn_string);
        return results;
    }

    // ========================================================================
    // INFO: Chạy reader_function.
    public static void exec_reader_function(
        string query,
        DatabaseConn.ReaderFunction f,
        string? conn_string = null
    )
    {
        ConnFunction conn_function = (conn) => DatabaseConn.exec_reader_function(conn, query, f);
        Database.exec_conn_function(conn_function, conn_string);
    }

    // ========================================================================
}

/* EOF */
