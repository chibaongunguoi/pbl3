// INFO: Các hàm thực hiện trên conn, reader cụ thể.

using Microsoft.Data.SqlClient;

class DatabaseConn
{
    // ========================================================================
    public delegate void ReaderFunction(SqlDataReader reader);
    public delegate void ConnReaderFunction(SqlConnection conn, SqlDataReader reader);

    // ========================================================================
    // INFO: Tạo reader từ query và truyền reader vào reader_function.
    public static void exec_reader_function(SqlConnection conn, string query, ReaderFunction f)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        using (SqlDataReader reader = command.ExecuteReader())
            while (reader.Read())
                f(reader);
    }

    // ------------------------------------------------------------------------
    // INFO: Tạo reader từ query và truyền reader vào reader_function.
    public static void exec_conn_reader_function(
        SqlConnection conn,
        string query,
        ConnReaderFunction f
    )
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        using (SqlDataReader reader = command.ExecuteReader())
            while (reader.Read())
                f(conn, reader);
    }

    // ========================================================================
    // INFO: Trả về các DataObj thu được từ query.
    public static List<T> exec_query<T>(SqlConnection conn, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        exec_reader_function(conn, query, (r) => DatabaseReader.read(r, ref results));
        return results;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về biểu diễn dạng string của các bản ghi thu được từ query.
    public static List<string> exec_query(SqlConnection conn, string query)
    {
        List<string> results = new List<string>();
        exec_reader_function(conn, query, (r) => DatabaseReader.read(r, ref results));
        return results;
    }

    // ========================================================================
    // INFO: Chạy non_query.
    public static void exec_non_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
            command.ExecuteNonQuery();
    }

    // ========================================================================
}

/* EOF */
