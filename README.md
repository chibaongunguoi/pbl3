# SqlConnection

Tình huống: Truy vấn dữ liệu từ CSDL

Bước 1: Tạo SqlConnection conn để kết nối tới CSDL
Bước 2: Truyền conn vào conn_function để thực hiện truy vấn dữ liệu
Bước 3: Đóng conn

```cs
    public delegate void ConnFunction(SqlConnection conn);
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
```

Trong đó:

`conn_function`: Một hàm nào đó nhận conn, hàm này sẽ được truyền tham số conn
ngay sau khi tạo conn.


`conn_function` cơ bản:

`exec_non_query`: chạy non-query (query không trả về dữ liệu)

```cs
    public static void exec_non_query(SqlConnection conn, string query)
    {
        using (SqlCommand command = new SqlCommand(query, conn))
        {
            command.ExecuteNonQuery();
        }
    }
```
`exec_reader`: Tạo một reader đọc các kết quả truy vấn rồi truyền reader vào reader function

```cs
    public delegate void ReaderFunction(SqlDataReader reader);
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
```

Ví dụ về reader function:

```cs
    void func(SqlDataReader reader)
    {
        string tch_name = DataReader.get_string(reader, 0);
        string sbj_name = DataReader.get_string(reader, 1);
        Console.WriteLine(tch_name + " - " + sbj_name);
    }
```
