using Microsoft.Data.SqlClient;

class JoinQuery {
    readonly string table;
    readonly string? alias_;
    readonly List<string> joins = [];
    public JoinQuery(string table, string? alias_ = null)
    {
        this.table = QPiece.tableAlias(table, alias_);
        this.alias_ = alias_;
    }

    public void AddField(string field_1, string field_2, string? alias_2 = null)
    {
        joins.Add(QPiece.eqField(QPiece.dot(field_1, alias_), QPiece.dot(field_2, alias_2)));
    }

    public void Add(string field_1, int value)
    {
        joins.Add(QPiece.eq(QPiece.dot(field_1, alias_), value));
    }

    private string GetOnClause()
    {
        return $"{table} ON {string.Join(" AND ", joins)}";
    }

    public string Join()
    {
        return $"INNER JOIN {GetOnClause()}";
    }

    public string LeftJoin()
    {
        return $"LEFT JOIN {GetOnClause()}";
    }
}

class Query
{
    // ========================================================================
    protected string? table = null;
    private List<string> output_fields = new();
    private List<string> conditions = new();
    private List<string> inner_joins = new();
    private List<string> order_bys = new();
    private List<string> set_fields = new();
    private List<string> group_bys = new();
    private string? offset_string = null;

    // ========================================================================
    public Query(string? table = null, string? alias = null)
    {
        this.table = table is not null ? QPiece.tableAlias(table, alias) : null;
    }

    // ========================================================================
    public void outputClause(params List<string> fields)
    {
        foreach (var field in fields)
        {
            if (field.Length > 0)
                output_fields.Add(field);
        }
    }

    public void outputAvgCastFloat(string field, string? alias = null)
    {
        outputClause(QPiece.avg(QPiece.castFloat(QPiece.dot(field, alias))));
    }

    public void outputAvg(string field, string? alias = null)
    {
        outputClause(QPiece.avg(QPiece.dot(field, alias)));
    }

    public void output(string field, string? alias = null)
    {
        outputClause(QPiece.dot(field, alias));
    }

    public void outputTop(string field_, int top = 1, string? alias_ = null)
    {
        outputClause($"TOP {top} {QPiece.dot(field_, alias_)}");
    }

    public void outputQuery(string query, string? alias_ = null)
    {
        if (alias_ is not null)
            outputClause($"({query}) AS {alias_}");
        else
            outputClause($"({query})");
    }

    public void groupByClause(params List<string> fields)
    {
        foreach (var field in fields)
        {
            if (field.Length > 0)
                group_bys.Add(field);
        }
    }

    public void groupBy(string field_, string? alias_ = null)
    {
        groupByClause(QPiece.dot(field_, alias_));
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
    public void Where<T>(string field, T value, string? alias_ = null)
    {
        WhereClause(QPiece.eq(QPiece.dot(field, alias_), value));
    }

    public void Where(string field, string value, string? alias_ = null)
    {
        WhereClause(QPiece.eq(QPiece.dot(field, alias_), value));
    }

    public void Where(string field, DateOnly value, string? alias_ = null)
    {
        WhereClause(QPiece.eq(QPiece.dot(field, alias_), value));
    }

    // ------------------------------------------------------------------------
    public void WhereQuery(string field, string query, string? alias_ = null)
    {
        WhereClause($"{QPiece.dot(field, alias_)} = ({query})");
    }

    public void WhereField(
        string field_1,
        string field_2,
        string? alias_1 = null,
        string? alias_2 = null
    )
    {
        WhereClause($"{QPiece.dot(field_1, alias_1)} = {QPiece.dot(field_2, alias_2)}");
    }

    // ------------------------------------------------------------------------
    public void WhereNStr(string field, string value, string? alias_ = null)
    {
        WhereClause($"{QPiece.dot(field, alias_)} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void Where<T>(string field, List<T> value, string? alias_ = null)
    {
        WhereClause(QPiece.inList(QPiece.dot(field, alias_), value));
    }

    public void Where(string field, List<string> value, string? alias_ = null)
    {
        WhereClause(QPiece.inList(QPiece.dot(field, alias_), value));
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

    public void OrderBy(string field_, bool desc = false, string? alias_ = null)
    {
        orderByClause(QPiece.orderBy(QPiece.dot(field_, alias_), desc: desc));
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

    public void Set(string field, int value, string? alias_ = null)
    {
        SetClause($"{QPiece.dot(field, alias_)} = {value}");
    }

    public void Set(string field, string value, string? alias_ = null)
    {
        SetClause($"{QPiece.dot(field, alias_)} = '{value}'");
    }

    public void Set(string field, DateOnly value, string? alias_ = null)
    {
        SetClause($"{QPiece.dot(field, alias_)} = {QPiece.toStr(value)}");
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

    public void Join(string field_1, string field_2, string? alias_1 = null, string? alias_2 = null)
    {
        string table_1 = field_1.Split('.')[0].Trim('[').Trim(']');
        JoinClause(
            QPiece.join(
                QPiece.tableAlias(table_1, alias_1),
                QPiece.dot(field_1, alias_1),
                QPiece.dot(field_2, alias_2)
            )
        );
    }

    public void LeftJoin(string field_1, string field_2, string? alias_1 = null, string? alias_2 = null)
    {
        string table_1 = field_1.Split('.')[0].Trim('[').Trim(']');
        JoinClause(
            QPiece.LeftJoin(
                QPiece.tableAlias(table_1, alias_1),
                QPiece.dot(field_1, alias_1),
                QPiece.dot(field_2, alias_2)
            )
        );
    }

    // ========================================================================
    public string GetWhereClause()
    {
        var conditions_str = string.Join(" AND ", conditions);
        string query = "";
        if (conditions_str.Length > 0)
            query += $" WHERE {conditions_str}";
        return query;
    }

    // ========================================================================
    private string GetJoinClause()
    {
        if (inner_joins.Count == 0)
            return "";
        return " " + string.Join(" ", inner_joins);
    }

    // ========================================================================
    private string GetGroupByClause()
    {
        string query = "";
        if (group_bys.Count > 0)
        {
            var s = string.Join(", ", group_bys);
            query += $" GROUP BY {s}";
        }
        return query;
    }

    // ========================================================================
    private string GetOrderClause()
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
    public string SelectQuery()
    {
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
        string query = $"SELECT {output_fields_str}";
        if (table is not null)
        {
            query += $" FROM {table}";
        }
        query += GetJoinClause();
        query += GetWhereClause();
        query += GetGroupByClause();
        query += GetOrderClause();
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
    public string DeleteQuery()
    {
        return $"DELETE FROM {table}" + GetWhereClause();
    }

    // ------------------------------------------------------------------------
    public string UpdateQuery()
    {
        string query = $"UPDATE {table} SET ";
        string set_fields_str = string.Join(", ", set_fields);
        query += set_fields_str + GetWhereClause();
        return query;
    }

    public string InsertQuery(string data)
    {
        string query = $"INSERT INTO {table} VALUES ({data})";
        return query;
    }

    // ========================================================================
    public void Select(SqlConnection conn, QDatabase.ReaderFunction f) =>
        QDatabase.execQuery(conn, SelectQuery(), f);

    // ------------------------------------------------------------------------
    public T Scalar<T>(SqlConnection conn)
    {
        SqlCommand command = new SqlCommand(SelectQuery(), conn);
        return (T)command.ExecuteScalar();
    }

    private int Scalar(SqlConnection conn)
    {
        return Scalar<int>(conn);
    }

    // ------------------------------------------------------------------------
    public void Delete(SqlConnection conn) => QDatabase.execQuery(conn, DeleteQuery());

    // ------------------------------------------------------------------------
    public void Insert(SqlConnection conn, string data) =>
        QDatabase.execQuery(conn, InsertQuery(data));

    // ------------------------------------------------------------------------
    public void Update(SqlConnection conn) => QDatabase.execQuery(conn, UpdateQuery());

    // ------------------------------------------------------------------------
    public List<T> Select<T>(SqlConnection conn)
        where T : DataObj, new()
    {
        List<T> results = new();
        QDatabase.execQuery(
            conn,
            SelectQuery(),
            reader => results.Add(DataReader.getDataObj<T>(reader))
        );
        return results;
    }

    public void Insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new()
    {
        Insert(conn, string.Join(", ", obj.ToList()));
    }

    public int Count(SqlConnection conn)
    {
        output(QPiece.countAll);
        return Scalar(conn);
    }

    // ========================================================================
}

/* EOF */
