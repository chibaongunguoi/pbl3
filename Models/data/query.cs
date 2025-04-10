using Microsoft.Data.SqlClient;

// INFO: Class tạo truy vấn
// Class truy vấn này được biệt hóa để sử dụng với kiểu
// dữ liệu Table (bảng) và Field (trường) được định nghĩa
// chỉ dành riêng cho dự án này.
sealed class Query
{
    // ========================================================================
    private Table table = new();
    private List<string> output_fields = new();
    private List<string> conditions = new();
    private List<string> inner_joins = new();
    private List<string> order_bys = new();
    private List<string> set_fields = new();
    private string? offset_string = null;

    // ========================================================================
    // INFO: Bắt đầu với một bảng
    public Query(Table table)
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
    public void offset(int page, int num_objs)
    {
        offset_string = $"OFFSET {(page - 1) * num_objs} ROWS FETCH NEXT {num_objs} ROWS ONLY";
    }

    // ========================================================================
    public void remove_offset()
    {
        offset_string = null;
    }

    // ========================================================================
    public void where_(Field table_field, int value)
    {
        conditions.Add($"{TableMngr.conv(table_field)} = {value}");
    }

    // ========================================================================
    public void where_(Field table_field, List<int> value)
    {
        string str = string.Join(", ", value);
        conditions.Add($"{TableMngr.conv(table_field)} IN ({str})");
    }

    // ========================================================================
    public void where_(Field table_field, List<string> value)
    {
        string str = string.Join(", ", value.Select(v => $"\'{v}\'"));
        conditions.Add($"{TableMngr.conv(table_field)} IN ({str})");
    }

    // ========================================================================
    public void where_string_contains(Field table_field, string value)
    {
        conditions.Add($"{TableMngr.conv(table_field)} LIKE '%{value}%'");
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

    // ------------------------------------------------------------------------
    public void order_by(Field table_field, bool desc = false)
    {
        string s = TableMngr.conv(table_field);
        if (desc)
            s += " DESC";
        order_bys.Add(s);
    }

    // ========================================================================
    public void set_(Field table_field, int value)
    {
        set_fields.Add($"{TableMngr.conv(table_field)} = {value}");
    }

    // ------------------------------------------------------------------------
    public void set_n(Field table_field, string value)
    {
        set_fields.Add($"{TableMngr.conv(table_field)} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void set_(Field table_field, string value)
    {
        set_fields.Add($"{TableMngr.conv(table_field)} = '{value}'");
    }

    // ========================================================================
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

    // ========================================================================
    public string get_join_clause()
    {
        if (inner_joins.Count == 0)
            return "";
        return " " + string.Join(" ", inner_joins);
    }

    // ========================================================================
    public string get_order_clause()
    {
        string query = "";
        if (order_bys.Count > 0)
        {
            var s = string.Join(", ", order_bys);
            query += $" ORDER BY {s}";
        }
        return query;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn SELECT
    public string get_select_query(bool count_mode = false)
    {
        var table_name = TableMngr.conv(table);
        var output_fields_str =
            output_fields.Count > 0 ? string.Join(", ", output_fields)
            : count_mode ? "COUNT(*)"
            : "*";
        string query = $"SELECT {output_fields_str} FROM {table_name}";
        query += get_join_clause();
        query += get_where_clause();
        query += get_order_clause();
        if (offset_string is not null)
        {
            if (order_bys.Count == 0)
                query += " ORDER BY (SELECT NULL)";
            query += " " + offset_string;
        }
        query += ";";
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
        List<string> parts = obj.ToListString();
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
            if (++pos == parts.Count)
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
        List<string> parts = obj.ToListString();
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
            if (++pos == parts.Count)
                break;
        }

        query = query.TrimEnd(',');
        query += $" WHERE {get_where_clause()};";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_update_query()
    {
        string query = $"UPDATE {TableMngr.conv(table)} SET ";
        string set_fields_str = string.Join(", ", set_fields);
        query += set_fields_str + get_where_clause() + ";";
        return query;
    }

    // ========================================================================
    // INFO: Các hàm truy vấn dưới đây có thêm tham số conn
    // Thực thi truy vấn SELECT, trả về list các DataObj
    public List<T> select<T>(SqlConnection conn)
        where T : DataObj, new()
    {
        return Database.exec_query<T>(conn, get_select_query());
    }

    // ------------------------------------------------------------------------
    // INFO: Thực thi truy vấn SELECT, trả về list các biểu diễn dạng xâu
    public List<string> select(SqlConnection conn) => Database.exec_query(conn, get_select_query());

    // ------------------------------------------------------------------------
    // INFO: Chạy reader function với truy vấn SELECT
    public void select(SqlConnection conn, Database.ReaderFunction f) =>
        Database.exec_reader(conn, get_select_query(), f);

    // ------------------------------------------------------------------------
    public int count(SqlConnection conn)
    {
        int result = 0;
        void func(SqlDataReader reader)
        {
            int pos = 0;
            result = DataReader.get_int(reader, ref pos);
        }
        Database.exec_reader(conn, get_select_query(count_mode: true), func);
        return result;
    }

    // ------------------------------------------------------------------------
    public void delete(SqlConnection conn) => Database.exec_non_query(conn, get_delete_query());

    // ------------------------------------------------------------------------
    public void insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => Database.exec_non_query(conn, get_insert_query<T>(obj));

    // ------------------------------------------------------------------------
    public void update<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => Database.exec_non_query(conn, get_update_query(obj));

    // ------------------------------------------------------------------------
    public void update(SqlConnection conn) => Database.exec_non_query(conn, get_update_query());

    // ========================================================================
}

/* EOF */
