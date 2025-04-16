using Microsoft.Data.SqlClient;

// INFO: Class tạo truy vấn
// Class truy vấn này được biệt hóa để sử dụng với kiểu
// dữ liệu Table (bảng) và Field (trường) được định nghĩa
// chỉ dành riêng cho dự án này.

static class QPiece
{
    public const string FALSE = "1 = 0";

    public static string eq(string field, int value) => $"{field} = {value}";

    public static string eqRaw(string field, string value) => $"{field} = {value}";

    public static string eq(string field, DateOnly value) =>
        $"{field} = '{IoUtils.conv_db(value)}'";

    public static string eq(string field, string value) => $"{field} = '{value}'";

    public static string eqNStr(string field, string value) => $"{field} = N'{value}'";

    public static string containsStr(string field, string value) => $"{field} LIKE '%{value}%'";

    public static string dot(string table, string field) => $"[{table}].[{field}]";

    public static string bracket(string s) => $"({s})";

    public static string castFloat(string field) => $"CAST({field} AS FLOAT)";

    public static string toStr(int value) => $"{value}";

    public static string toStr(DateOnly value) => $"'{IoUtils.conv_db(value)}'";

    public static string toStr(string value) => $"\'{value}\'";

    public static string toNStr(string value) => $"N\'{value}\'";

    public static string avg(string field, string? alias = null)
    {
        string s = $"ISNULL(AVG({field}), 0)";
        if (alias is null)
            return s;
        return $"{s} AS {alias}";
    }

    public static string alias(string field, string? alias)
    {
        if (alias is null)
            return field;
        return $"{field} AS {alias}";
    }

    public const string countAll = "COUNT(*)";

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

sealed class Query
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
    public Query(string table)
    {
        this.table = table;
    }

    // ========================================================================
    // INFO: Thêm trường vào danh sách trường cần lấy
    public void outputClause(params List<string> fields)
    {
        foreach (var field in fields)
        {
            if (field.Length > 0)
                output_fields.Add(field);
        }
    }

    public void outputAvg(string field)
    {
        outputClause(QPiece.avg(field));
    }

    public void outputAvg(string table, string field)
    {
        outputClause(QPiece.avg(QPiece.dot(table, field)));
    }

    public void output(string table, string field)
    {
        outputClause(QPiece.dot(table, field));
    }

    // ========================================================================
    public void offset(int page, int num_objs)
    {
        offset_string = $"OFFSET {(page - 1) * num_objs} ROWS FETCH NEXT {num_objs} ROWS ONLY";
    }

    // ========================================================================
    public void removeOffset()
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

    public void Where(string field, int value)
    {
        WhereClause($"{field} = {value}");
    }

    public void Where(string field, string value)
    {
        WhereClause($"{field} = '{value}'");
    }

    public void WhereRaw(string table, string field, string value)
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

    public void Where(string table, string field, string value)
    {
        WhereClause($"{QPiece.dot(table, field)} = '{value}'");
    }

    public void WhereNStr(string table, string field, string value)
    {
        WhereClause($"{QPiece.dot(table, field)} = N'{value}'");
    }

    public void Where(string table, string field, List<string> value)
    {
        WhereClause(QPiece.inStrList(QPiece.dot(table, field), value));
    }

    // ========================================================================
    public void orderByClause(params List<string> order_by)
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

    public void Set(string table, string field, string value)
    {
        SetClause($"{QPiece.dot(table, field)} = '{value}'");
    }

    public void Set(string table, string field, int value)
    {
        SetClause($"{QPiece.dot(table, field)} = {value}");
    }

    public void Set(string field, int value)
    {
        SetClause($"{field} = {value}");
    }

    public void Set(string field, string value)
    {
        SetClause($"{field} = '{value}'");
    }

    // ------------------------------------------------------------------------
    public void JoinClause(params List<string> join_cmd)
    {
        foreach (var cmd in join_cmd)
        {
            if (cmd.Length > 0)
                inner_joins.Add(cmd);
        }
    }

    public void join(string table_1, string field_1, string table_2, string field_2)
    {
        JoinClause(
            QPiece.join(table_1, QPiece.dot(table_1, field_1), QPiece.dot(table_2, field_2))
        );
    }

    // ========================================================================
    public string getWhereClause()
    {
        var conditions_str = string.Join(" AND ", conditions);
        string query = "";
        if (conditions_str.Length > 0)
            query += $" WHERE {conditions_str}";
        return query;
    }

    // ========================================================================
    private string getJoinClause()
    {
        if (inner_joins.Count == 0)
            return "";
        return " " + string.Join(" ", inner_joins);
    }

    // ========================================================================
    private string getOrderClause()
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
    public string selectQuery(bool count_mode = false)
    {
        var output_fields_str =
            count_mode ? "COUNT(*)"
            : output_fields.Count > 0 ? string.Join(", ", output_fields)
            : "*";
        string query = $"SELECT {output_fields_str} FROM {table}";
        query += getJoinClause();
        query += getWhereClause();
        query += getOrderClause();
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
    public string deleteQuery()
    {
        return $"DELETE FROM {table}" + getWhereClause();
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn INSERT
    public string insertQuery<T>(T obj)
        where T : DataObj, new()
    {
        List<string> parts = obj.ToListString();
        string query = $"INSERT INTO {table} VALUES ({string.Join(", ", parts)})";
        return query;
    }

    // ------------------------------------------------------------------------
    public string updateQuery()
    {
        string query = $"UPDATE {table} SET ";
        string set_fields_str = string.Join(", ", set_fields);
        query += set_fields_str + getWhereClause();
        return query;
    }

    // ========================================================================
    public List<T> select<T>(SqlConnection conn)
        where T : DataObj, new()
    {
        return Database.exec_query<T>(conn, selectQuery());
    }

    // ------------------------------------------------------------------------
    public void select(SqlConnection conn, Database.ReaderFunction f) =>
        Database.exec_reader(conn, selectQuery(), f);

    // ------------------------------------------------------------------------
    public int count(SqlConnection conn)
    {
        int result = 0;
        void func(SqlDataReader reader)
        {
            int pos = 0;
            result = DataReader.get_int(reader, ref pos);
        }
        Database.exec_reader(conn, selectQuery(count_mode: true), func);
        return result;
    }

    // ------------------------------------------------------------------------
    public void delete(SqlConnection conn) => Database.exec_non_query(conn, deleteQuery());

    // ------------------------------------------------------------------------
    public void insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => Database.exec_non_query(conn, insertQuery<T>(obj));

    // ------------------------------------------------------------------------
    public void update(SqlConnection conn) => Database.exec_non_query(conn, updateQuery());

    // ========================================================================
}

/* EOF */
