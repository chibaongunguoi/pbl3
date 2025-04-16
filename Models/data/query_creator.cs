using Microsoft.Data.SqlClient;

// INFO: Class tạo truy vấn
// Class truy vấn này được biệt hóa để sử dụng với kiểu
// dữ liệu Table (bảng) và Field (trường) được định nghĩa
// chỉ dành riêng cho dự án này.

static class QPiece
{
    public const string FALSE = "1 = 0";

    public static string eq(string field, int value) => $"{field} = {value}";

    public static string eq(string field, string value) => $"{field} = {value}";

    public static string eq(string field, DateOnly value) =>
        $"{field} = '{IoUtils.conv_db(value)}'";

    public static string eqStr(string field, string value) => $"{field} = '{value}'";

    public static string eqNStr(string field, string value) => $"{field} = N'{value}'";

    public static string containsStr(string field, string value) => $"{field} LIKE '%{value}%'";

    public static string dot(string table, string field) => $"[{table}].[{field}]";

    public static string bracket(string s) => $"({s})";

    public static string cast_float(string field) => $"CAST({field} AS FLOAT)";

    public static string avg(string field, string? alias = null)
    {
        if (alias is null)
            return $"AVG({field})";
        return $"AVG({field}) AS {alias}";
    }

    public static string alias(string field, string? alias)
    {
        if (alias is null)
            return field;
        return $"{field} AS {alias}";
    }

    public const string countAll = "COUNT(*)";

    public static string eqDate(string field, DateOnly value) =>
        $"{field} = '{IoUtils.conv_db(value)}'";

    public static string inStrList(string field, List<string> value)
    {
        if (value.Count == 0)
            return FALSE;
        string str = string.Join(", ", value.Select(v => $"\'{v}\'"));
        return $"{field} IN ({str})";
    }

    public static string inIntList(string field, List<string> value)
    {
        if (value.Count == 0)
            return FALSE;
        string str = string.Join(", ", value);
        return $"{field} IN ({str})";
    }

    public static string orderBy(string field, bool desc = false)
    {
        string s = field;
        if (desc)
            s += " DESC";
        return s;
    }

    public static string join(string table, string field_1, string field_2)
    {
        return $"INNER JOIN {table} ON {field_1} = {field_2}";
    }
}

// static class QShort
// {
//     public static string eq(string table, string field, int value) =>
//         QPiece.eq(QPiece.dot(table, field), value);
//
//     public static string eq(string table, string field, string value) =>
//         QPiece.eq(QPiece.dot(table, field), value);
//
//     public static string eq(string table, string field, DateOnly value) =>
//         QPiece.eq(QPiece.dot(table, field), value);
//
//     public static string eq(string table_1, string field_1, string table_2, string field_2) =>
//         QPiece.eq(QPiece.dot(table_1, field_1), QPiece.dot(table_2, field_2));
//
//     public static string eqStr(string table, string field, string value) =>
//         QPiece.eqStr(QPiece.dot(table, field), value);
//
//     public static string eqNStr(string table, string field, string value) =>
//         QPiece.eqNStr(QPiece.dot(table, field), value);
//
//     public static string join(string table_1, string field_1, string table_2, string field_2)
//     {
//         return QPiece.join(table_1, QPiece.dot(table_1, field_1), QPiece.dot(table_2, field_2));
//     }
//
//     public static string inStrList(string table, string field, List<string> value)
//     {
//         return QPiece.inStrList(QPiece.dot(table, field), value);
//     }
// }

sealed class QueryCreator
{
    // ========================================================================
    private string table;
    private List<string> output_fields = new();
    private List<string> conditions = new();
    private List<string> inner_joins = new();
    private List<string> order_bys = new();
    private List<string> set_fields = new();
    private string? offset_string = null;

    // ========================================================================
    // INFO: Bắt đầu với một bảng
    public QueryCreator(string table)
    {
        this.table = table;
    }

    // ========================================================================
    // INFO: Thêm trường vào danh sách trường cần lấy
    public void output(params List<string> fields)
    {
        foreach (var field in fields)
        {
            if (field.Length > 0)
                output_fields.Add(field);
        }
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
    public void WhereClause(params List<string> condition)
    {
        foreach (var cond in condition)
        {
            if (cond.Length > 0)
                conditions.Add(cond);
        }
    }

    public void Where(string table, string field, int value)
    {
        WhereClause($"{QPiece.dot(table, field)} = {value}");
    }

    public void Where(string table, string field, string value)
    {
        WhereClause($"{QPiece.dot(table, field)} = {value}");
    }

    public void Where(string table, string field, DateOnly value)
    {
        WhereClause($"{QPiece.dot(table, field)} = '{IoUtils.conv_db(value)}'");
    }

    public void Where(string table, string field, string table_2, string field_2)
    {
        WhereClause($"{QPiece.dot(table, field)} = {QPiece.dot(table_2, field_2)}");
    }

    public void WhereStr(string table, string field, string value)
    {
        WhereClause($"{QPiece.dot(table, field)} = '{value}'");
    }

    public void WhereNStr(string table, string field, string value)
    {
        WhereClause($"{QPiece.dot(table, field)} = N'{value}'");
    }

    public void WhereStrList(string table, string field, List<string> value)
    {
        WhereClause(QPiece.inStrList(QPiece.dot(table, field), value));
    }

    // ========================================================================
    // ------------------------------------------------------------------------
    public void orderBy(params List<string> order_by)
    {
        foreach (var field in order_by)
        {
            if (field.Length > 0)
                order_bys.Add(field);
        }
    }

    // ========================================================================
    public void SetClause(params List<string> set_fields)
    {
        foreach (var field in set_fields)
        {
            if (field.Length > 0)
                this.set_fields.Add(field);
        }
    }

    public void SetStr(string table, string field, string value)
    {
        SetClause($"{QPiece.dot(table, field)} = '{value}'");
    }

    // ------------------------------------------------------------------------
    // INFO: Phép kết tự cấu hình
    public void JoinClause(params List<string> join_cmd)
    {
        foreach (var cmd in join_cmd)
        {
            if (cmd.Length > 0)
                inner_joins.Add(cmd);
        }
    }

    public void Join(string table_1, string field_1, string table_2, string field_2)
    {
        JoinClause(
            QPiece.join(table_1, QPiece.dot(table_1, field_1), QPiece.dot(table_2, field_2))
        );
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
        var output_fields_str =
            count_mode ? "COUNT(*)"
            : output_fields.Count > 0 ? string.Join(", ", output_fields)
            : "*";
        string query = $"SELECT {output_fields_str} FROM {table}";
        query += get_join_clause();
        query += get_where_clause();
        query += get_order_clause();
        if (offset_string is not null)
        {
            if (order_bys.Count == 0)
                query += " ORDER BY (SELECT NULL)";
            query += " " + offset_string;
        }
        return query;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn DELETE
    public string get_delete_query()
    {
        return $"DELETE FROM {table}" + get_where_clause();
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn INSERT
    public string get_insert_query<T>(T obj, Table table)
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
        query += ")";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_update_query()
    {
        string query = $"UPDATE {table} SET ";
        string set_fields_str = string.Join(", ", set_fields);
        query += set_fields_str + get_where_clause();
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
    public void insert<T>(SqlConnection conn, T obj, Table tbl)
        where T : DataObj, new() => Database.exec_non_query(conn, get_insert_query<T>(obj, tbl));

    // ------------------------------------------------------------------------
    public void update(SqlConnection conn) => Database.exec_non_query(conn, get_update_query());

    // ========================================================================
}

/* EOF */
