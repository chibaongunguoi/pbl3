using Microsoft.Data.SqlClient;

// INFO: Class tạo truy vấn
// Class truy vấn này được biệt hóa để sử dụng với kiểu
// dữ liệu Table (bảng) và Field (trường) được định nghĩa
// chỉ dành riêng cho dự án này.
sealed class Query
{
    // ========================================================================
    private Table table = new();
    private List<string> output_fields = new List<string>();
    private List<string> conditions = new List<string>();
    private List<string> inner_joins = new List<string>();
    private string? conn_string = null;

    // ========================================================================
    // INFO: Bắt đầu với một bảng
    public Query(Table table, string? conn_string = null)
    {
        this.table = table;
    }

    // ========================================================================
    // INFO: Thêm trường vào danh sách trường cần lấy
    public void output(Field table_field)
    {
        output_fields.Add(TableMngr.conv(table_field));
    }

    // ========================================================================
    public void where_(Field table_field, int value)
    {
        conditions.Add($"{TableMngr.conv(table_field)} = {value}");
    }

    // ------------------------------------------------------------------------
    // INFO: Thêm xâu unicode vào điều kiện
    public void where_n(Field table_field, string value)
    {
        conditions.Add($"{TableMngr.conv(table_field)} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(Field table_field, string value)
    {
        conditions.Add($"{TableMngr.conv(table_field)} = '{value}'");
    }

    // ------------------------------------------------------------------------
    // INFO: Điều kiện tự cấu hình
    public void where_(string condition)
    {
        conditions.Add(condition);
    }

    // ========================================================================

    // ------------------------------------------------------------------------
    public void join(Field field_1_, Field field_2_)
    {
        string table_name_1 = TableMngr.conv(TableMngr.get_table(field_1_));
        var field_1 = TableMngr.conv(field_1_);
        var field_2 = TableMngr.conv(field_2_);
        inner_joins.Add($"INNER JOIN {table_name_1} ON {field_1} = {field_2}");
    }

    // ------------------------------------------------------------------------
    // INFO: Phép kết tự cấu hình
    public void join(string join_cmd)
    {
        inner_joins.Add(join_cmd);
    }

    // ========================================================================
    public string get_where_clause()
    {
        var conditions_str = string.Join(" AND ", conditions);
        string query = "";
        if (conditions_str.Length > 0)
            query += $" WHERE {conditions_str}";
        return query;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn SELECT
    public string get_select_query()
    {
        var table_name = TableMngr.conv(table);
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
        string query = $"SELECT {output_fields_str} FROM {table_name} ";
        query += string.Join(" ", inner_joins);
        query += get_where_clause() + ";";
        return query;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn DELETE
    public string get_delete_query()
    {
        var table_name = TableMngr.conv(table);
        return $"DELETE FROM {table_name}" + get_where_clause() + ";";
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn INSERT
    public string get_insert_query<T>(T obj)
        where T : DataObj, new()
    {
        var table_config = TableMngr.get_table_config(table);
        string[] parts = obj.ToString().Split(',');
        string query = $"INSERT INTO {table_config.name} VALUES (";
        int pos = 0;
        foreach (var field in table_config.fields)
        {
            switch (field.dtype)
            {
                case "INT":
                    query += $"{parts[pos]},";
                    break;
                case "NSTRING":
                    query += $"N'{parts[pos]}',";
                    break;
                case "STRING":
                    query += $"'{parts[pos]}',";
                    break;
            }
            if (++pos == parts.Length)
                break;
        }

        query = query.TrimEnd(',');
        query += ");";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_update_query<T>(T obj)
        where T : DataObj, new()
    {
        var table_config = TableMngr.get_table_config(table);
        string[] parts = obj.ToString().Split(',');
        string query = $"UPDATE {table_config.name} SET ";
        int pos = 0;
        foreach (var field in table_config.fields)
        {
            switch (field.dtype)
            {
                case "INT":
                    query += $"{field.name} = {parts[pos]},";
                    break;
                case "NSTRING":
                    query += $"{field.name} = N'{parts[pos]}',";
                    break;
                case "STRING":
                    query += $"{field.name} = '{parts[pos]}',";
                    break;
            }
            if (++pos == parts.Length)
                break;
        }

        query = query.TrimEnd(',');
        query += $" WHERE {get_where_clause()};";
        return query;
    }

    // ========================================================================
    // INFO: Các hàm truy vấn dưới đây có thêm tham số conn
    // Thực thi truy vấn SELECT, trả về list các DataObj
    public List<T> select<T>(SqlConnection conn)
        where T : DataObj, new() => DatabaseConn.exec_query<T>(conn, get_select_query());

    // ------------------------------------------------------------------------
    // INFO: Thực thi truy vấn SELECT, trả về list các biểu diễn dạng xâu
    public List<string> select(SqlConnection conn) =>
        DatabaseConn.exec_query(conn, get_select_query());

    // ------------------------------------------------------------------------
    // INFO: Chạy reader function với truy vấn SELECT
    public void select(SqlConnection conn, DatabaseConn.ReaderFunction f) =>
        DatabaseConn.exec_reader_function(conn, get_select_query(), f);

    // ------------------------------------------------------------------------
    public void delete(SqlConnection conn) => DatabaseConn.exec_non_query(conn, get_delete_query());

    // ------------------------------------------------------------------------
    public void insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => DatabaseConn.exec_non_query(conn, get_insert_query<T>(obj));

    // ------------------------------------------------------------------------
    public void update<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => DatabaseConn.exec_non_query(conn, get_update_query(obj));

    // ========================================================================
    public List<T> select<T>()
        where T : DataObj, new() => Database.exec_query<T>(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public List<string> select() => Database.exec_query(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public void select(DatabaseConn.ReaderFunction f) =>
        Database.exec_reader_function(get_select_query(), f, conn_string);

    // ------------------------------------------------------------------------
    public void delete() => Database.exec_non_query(get_delete_query(), conn_string);

    // ------------------------------------------------------------------------
    public void insert<T>(T obj)
        where T : DataObj, new() => Database.exec_non_query(get_insert_query<T>(obj), conn_string);

    // ------------------------------------------------------------------------
    public void update<T>(T obj)
        where T : DataObj, new() => Database.exec_non_query(get_update_query<T>(obj), conn_string);

    // ========================================================================
}

/* EOF */
