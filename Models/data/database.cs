using Microsoft.Data.SqlClient;

class Database
{
    // ========================================================================
    // INFO: Delegate cho các hàm nhận conn làm tham số.
    public delegate void ConnFunction(SqlConnection conn);
    public delegate List<T> ConnFunction<T>(SqlConnection conn);

    // ========================================================================
    // INFO: Tạo mới conn và truyền conn vào ConnFunction, sau đó ngắt kết nối.
    public static void exec(ConnFunction conn_function, string? conn_string = null)
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

    // ------------------------------------------------------------------------
    public static List<T> exec_list<T>(ConnFunction<T> conn_function, string? conn_string = null)
    {
        List<T> results = new();
        exec(conn => results = conn_function(conn), conn_string);
        return results;
    }

    // ========================================================================
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Chạy non_query (query kiểu hành động, không trả về dữ liệu)
    public static void exec_non_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            command.ExecuteNonQuery();
        }
    }

    // ========================================================================
    // INFO:
    // ReaderFunction là một hàm nhận một reader làm tham số.
    // ReaderFunction sẽ liên tục được truyền reader tương ứng cho mỗi bản ghi của kết quả
    // truy vấn, khi đó nó có thể đọc dữ liệu của mỗi bản ghi và sẽ có hành động nhất định
    // với dữ liệu đọc được. Ví dụ, một reader_function mỗi lần nhận id và name
    // mới thì sẽ thêm id và name đó vào một list kết quả.
    public delegate void ReaderFunction(SqlDataReader reader);

    // ========================================================================
    // INFO: Tạo reader và truyền reader vào reader_function.
    public static void exec_reader(SqlConnection conn, string query, ReaderFunction f)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    f(reader);
                }
            }
        }
    }

    // ========================================================================
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Trả về các DataObj thu được từ query.
    public static List<T> exec_query<T>(SqlConnection conn, string query)
        where T : DataObj, new()
    {
        List<T> results = new List<T>();
        void func(SqlDataReader reader)
        {
            int pos = 0;
            results.Add(DataReader.get_data_obj<T>(reader, ref pos));
        }
        Database.exec_reader(conn, query, func);
        return results;
    }

    // ------------------------------------------------------------------------
    // INFO:
    // Đây là hàm chạy trên một conn cụ thể.
    // Trả về biểu diễn dạng xâu của các bản ghi thu được từ query.
    public static List<string> exec_query(SqlConnection conn, string query)
    {
        List<string> results = new List<string>();
        void func(SqlDataReader reader)
        {
            results.Add(string.Join(",", DataReader.get_sql_repr(reader)));
        }
        Database.exec_reader(conn, query, func);
        return results;
    }

    // ========================================================================
}

/* EOF */
