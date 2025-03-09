using Microsoft.Data.SqlClient;

// INFO: Tập hợp các hàm có nhiệm vụ tạo mới một kết nối (connection)
// để thực thi các query, sau đó ngắt kết nối.
// Nếu muốn thực thi query trong một kết nối cụ thể mà không
// muốn ngắt kết nối đó, hãy sử dụng class DatabaseConn.
class Database
{
    // ========================================================================
    // INFO: Delegate cho các hàm nhận conn làm tham số.
    public delegate void ConnFunction(SqlConnection conn);

    // ========================================================================
    // INFO: Tạo mới conn và truyền conn vào ConnFunction, sau đó ngắt kết nối.
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
    // INFO: Chạy non_query trong một conn mới rồi ngắt kết nối.
    // Non_query là các câu lệnh không trả về kết quả, như INSERT, UPDATE, DELETE.
    public static void exec_non_query(string query, string? conn_string = null)
    {
        Database.exec_conn_function(conn => DatabaseConn.exec_non_query(conn, query), conn_string);
    }

    // ========================================================================
    // INFO: Chạy query trong một conn mới rồi ngắt kết nối.
    // Trả về các DataObj thu được từ query đó.
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
    // INFO:
    // Trả về biểu diễn dạng xâu
    // của các bản ghi thu được từ query.
    // (khuyến cáo: kết quả không giống với kết quả của ToString đối với DataObj)
    public static List<string> exec_query(string query, string? conn_string = null)
    {
        List<string> results = new List<string>();
        ConnFunction conn_function = (conn) => results = DatabaseConn.exec_query(conn, query);
        Database.exec_conn_function(conn_function, conn_string);
        return results;
    }

    // ========================================================================
    // INFO:
    // Tạo mới conn, thực thi query và gọi reader_function trong conn đó rồi ngắt kết nối.
    // Về reader_function, xem chi tiết tại class DatabaseConn.
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
