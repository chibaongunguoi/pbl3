using Microsoft.Data.SqlClient;

// Note: Field trong dự án này có format: Table.Field
static class QPiece
{
    public const string FALSE = "1 = 0";

    public static string containsStr(string field, string value) => $"{field} LIKE '%{value}%'";

    public static string dot(string table, string field) => $"[{table}].[{field}]";

    public static string castFloat(string field) => $"CAST({field} AS FLOAT)";

    public static string toStr(int value) => $"{value}";

    public static string toStr(DateOnly value) => $"'{value.Year}-{value.Month:D2}-{value.Day:D2}'";

    public static string toStr(DateTime value) =>
        $"'{value.Year}-{value.Month:D2}-{value.Day:D2} {value.Hour:D2}:{value.Minute:D2}:{value.Second:D2}'";

    public static string toStr(string value) => $"\'{value}\'";

    public static string toNStr(string value) => $"N\'{value}\'";

    public static string avg(string field)
    {
        return $"ISNULL(AVG({field}), 0)";
    }

    public static string alias(string field, string? alias)
    {
        if (alias is null)
            return field;
        return $"{field} AS {alias}";
    }

    public static string dotAlias(string field, string? alias = null)
    {
        if (alias is null)
        {
            return field;
        }
        field = field.Split('.')[1];
        return $"{alias}.{field}";
    }

    public const string countAll = "COUNT(*)";

    public static string allFieldsOf(string table) => $"[{table}].*"; // select all

    // ------------------------------------------------------------------------
    public static string eq<T>(string field, T value, string op = "=")
    {
        return $"{field} {op} {value}";
    }

    public static string eq(string field, string value, string op = "=")
    {
        return $"{field} {op} '{value}'";
    }

    public static string eq(string field, DateOnly value, string op = "=")
    {
        return $"{field} {op} {QPiece.toStr(value)}";
    }

    public static string eq(string field, DateTime value, string op = "=")
    {
        return $"{field} {op} {QPiece.toStr(value)}";
    }

    // ------------------------------------------------------------------------
    public static string inList<T>(string field, List<T> values) // thực hiện pehps IN
    {
        if (values.Count == 0)
            return FALSE;
        string str = string.Join(", ", values);
        return $"{field} IN ({str})";
    }

    public static string inList(string field, List<string> values)
    {
        values = values.Select(v => $"\'{v}\'").ToList();
        return inList<string>(field, values);
    }

    // ------------------------------------------------------------------------
    public static string orderBy(string field, bool desc = false)
    {
        string s = field;
        if (desc)
            s += " DESC";
        return s;
    }

    public static string join(string table_1, string field_1, string field_2)
    {
        return $"INNER JOIN {table_1} ON {field_1} = {field_2}";
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
    public Query(string table, string? alias = null)
    {
        if (alias is not null)
            this.table = QPiece.alias(table, alias);
        else
            this.table = table;
    }

    public Query()
    {
        this.table = "";
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

    public void outputAvgCastFloat(string field, string? alias = null)
    {
        outputClause(QPiece.avg(QPiece.castFloat(QPiece.dotAlias(field, alias))));
    }

    public void outputAvg(string table, string field)
    {
        outputClause(QPiece.avg(QPiece.dot(table, field)));
    }

    public void output(string table, string field)
    {
        outputClause(QPiece.dot(table, field));
    }

    public void output(string field)
    {
        outputClause(field);
    }

    public void outputTop(string field, int top = 1)
    {
        outputClause($"TOP {top} {field}");
    }

    public void outputTopAlias(string field_, string? alias_, int top = 1)
    {
        outputClause($"TOP {top} {QPiece.dotAlias(field_, alias_)}");
    }

    public void outputQuery(string query)
    {
        outputClause($"({query})");
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

    // ------------------------------------------------------------------------
    public void Where<T>(string field, T value)
    {
        WhereClause(QPiece.eq(field, value));
    }

    public void Where(string field, string value)
    {
        WhereClause(QPiece.eq(field, value));
    }

    public void Where(string field, DateOnly value)
    {
        WhereClause(QPiece.eq(field, value));
    }

    // ------------------------------------------------------------------------
    public void WhereField(string field_1, string field_2)
    {
        WhereClause($"{field_1} = {field_2}");
    }

    public void WhereQuery(string field, string query)
    {
        WhereClause($"{field} = ({query})"); // ==> query trả về một giá trị duy nhất (MAX, AVG,...)
    }

    public void WhereFieldAlias(
        string field_1,
        string? alias_1,
        string field_2,
        string? alias_2 = null
    )
    {
        field_2 = alias_2 is null ? field_2 : QPiece.dotAlias(field_2, alias_2);
        WhereClause($"{QPiece.dotAlias(field_1, alias_1)} = {field_2}");
    }

    // ------------------------------------------------------------------------
    public void WhereNStr(string field, string value)
    {
        WhereClause($"{field} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void Where<T>(string field, List<T> value)
    {
        WhereClause(QPiece.inList(field, value));
    }

    public void Where(string field, List<string> value)
    {
        WhereClause(QPiece.inList(field, value));
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

    public void orderBy(string field, bool desc = false)
    {
        orderByClause(QPiece.orderBy(field, desc: desc));
    }

    public void orderByAlias(string field_, string? alias_, bool desc = false)
    {
        orderByClause(QPiece.orderBy(QPiece.dotAlias(field_, alias_), desc: desc));
    }

    // ========================================================================
    public void SetClause(params List<string> set_fields) // thêm A = ..., B = ... vào danh sách set_fields
    {
        foreach (var field in set_fields)
        {
            if (field.Length > 0)
                this.set_fields.Add(field);
        }
    }

    public void Set(string field, int value)
    {
        SetClause($"{field} = {value}");
    }

    public void Set(string field, string value) // set 1 trường cho string
    {
        SetClause($"{field} = '{value}'");
    }

    // ------------------------------------------------------------------------
    public void JoinClause(params List<string> join_cmd) // ADD(Join:B on ....) vào danh sách inner_joins
    {
        foreach (var cmd in join_cmd)
        {
            if (cmd.Length > 0)
                inner_joins.Add(cmd);
        }
    }

    public void join(string field_1, string field_2)
    {
        string table_1 = field_1.Split('.')[0];
        JoinClause(QPiece.join(table_1, field_1, field_2));
    }

    public void join(string table, string field_1, string field_2) // đài vào field có dạng Table.Field => mục đích trả về phép Join table Field1 = field2
    {
        JoinClause(QPiece.join(table, field_1, field_2));
    }

    public void joinAlias(string field_1, string? alias_1, string field_2, string? alias_2 = null)
    {
        string table_1 = field_1.Split('.')[0];
        JoinClause(
            QPiece.join(
                QPiece.alias(table_1, alias_1),
                QPiece.dotAlias(field_1, alias_1),
                QPiece.dotAlias(field_2, alias_2)
            )
        );
    }

    // ========================================================================
    public string getWhereClause() // thực hiện phép And giữa các điều kiện rôi trả về điều kiện WHERE
    {
        var conditions_str = string.Join(" AND ", conditions);
        string query = "";
        if (conditions_str.Length > 0)
            query += $" WHERE {conditions_str}";
        return query;
    }

    // ========================================================================
    private string getJoinClause() // chưa hiện hiện Form A
    {
        if (inner_joins.Count == 0)
            return "";
        return " " + string.Join(" ", inner_joins);
    }

    // ========================================================================
    private string getOrderClause() // trả về câu lệnh ORDER BY hoàn chỉnh
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
    public string selectQuery(bool count_mode = false) // nếu là true sẽ dùng countAll, ngược lại lấy lệnh truy vấn hoanf chỉnh
    {
        var output_fields_str =
            count_mode ? QPiece.countAll
            : output_fields.Count > 0 ? string.Join(", ", output_fields)
            : "*";
        string query = $"SELECT {output_fields_str}";
        if (table.Length > 0)
            query += $" FROM {table}";
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
    public string insertQuery<T>(T obj) // lấy lệnh insert
        where T : DataObj, new()
    {
        List<string> parts = obj.ToList();
        string query = $"INSERT INTO {table} VALUES ({string.Join(",", parts)})";
        return query;
    }

    // ------------------------------------------------------------------------
    public string updateQuery() // lấy lệnh update
    {
        string query = $"UPDATE {table} SET ";
        string set_fields_str = string.Join(", ", set_fields);
        query += set_fields_str + getWhereClause();
        return query;
    }

    // ========================================================================
    public List<T> select<T>(SqlConnection conn) // trả về một list các đối tượng T kế thừa từ Obj, mỗi đối tượng T là một recor trong Database
        where T : DataObj, new()
    {
        return Database.execQuery<T>(conn, selectQuery());
    }

    // ------------------------------------------------------------------------
    public void select(SqlConnection conn, Database.ReaderFunction f) =>
        Database.execQuery(conn, selectQuery(), f);

    // ------------------------------------------------------------------------
    public int count(SqlConnection conn)
    {
        int result = 0;
        Database.execQuery(
            conn,
            selectQuery(count_mode: true),
            reader => result = DataReader.getInt(reader)
        );
        return result;
    }

    // ------------------------------------------------------------------------
    public void delete(SqlConnection conn) => Database.execQuery(conn, deleteQuery());

    // ------------------------------------------------------------------------
    public void insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => Database.execQuery(conn, insertQuery<T>(obj));

    // ------------------------------------------------------------------------
    public void update(SqlConnection conn) => Database.execQuery(conn, updateQuery());

    // ========================================================================
}

/* EOF */
