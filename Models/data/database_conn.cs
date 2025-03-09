// INFO: Các hàm thực hiện trên conn, reader cụ thể.

using Microsoft.Data.SqlClient;

class DatabaseConn
{
    // ========================================================================
    // INFO:
    // ReaderFunction là một hàm nhận một reader làm tham số.
    // ReaderFunction sẽ liên tục được truyền reader tương ứng cho mỗi bản ghi của kết quả
    // truy vấn, khi đó nó có thể đọc dữ liệu của mỗi bản ghi và sẽ có hành động nhất định
    // với dữ liệu đọc được. Ví dụ, một reader_function mỗi lần nhận id và name
    // mới thì sẽ thêm id và name đó vào một list kết quả.
    public delegate void ReaderFunction(SqlDataReader reader);

    // INFO:
    // ConnReaderFunction là một hàm nhận một conn và một reader làm tham số.
    // Cách hoạt động giống hệt ReaderFunction, nhưng ConnReaderFunction
    // cho phép thêm một tham số conn, giúp thực hiện các hành động cần thiết
    // đối với conn đó.
    public delegate void ConnReaderFunction(SqlConnection conn, SqlDataReader reader);

    // ========================================================================
    // INFO: Tạo reader và truyền reader vào reader_function.
    public static void exec_reader_function(SqlConnection conn, string query, ReaderFunction f)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        using (SqlDataReader reader = command.ExecuteReader())
            while (reader.Read())
                f(reader);
    }

    // ------------------------------------------------------------------------
    // INFO: Tạo reader và truyền reader vào conn_reader_function.
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
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Trả về các DataObj thu được từ query.
    public static List<T> exec_query<T>(SqlConnection conn, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        exec_reader_function(conn, query, (r) => DatabaseReader.read(r, ref results));
        return results;
    }

    // ------------------------------------------------------------------------
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Trả về biểu diễn dạng xâu của các bản ghi thu được từ query.
    // Xem lưu ý được ghi tại class Database.
    public static List<string> exec_query(SqlConnection conn, string query)
    {
        List<string> results = new List<string>();
        exec_reader_function(conn, query, (r) => DatabaseReader.read(r, ref results));
        return results;
    }

    // ========================================================================
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Chạy non_query (query kiểu hành động, không trả về dữ liệu)
    public static void exec_non_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
            command.ExecuteNonQuery();
    }

    // ========================================================================
}

/* EOF */
