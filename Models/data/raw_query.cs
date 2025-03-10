using Microsoft.Data.SqlClient;

sealed class RawQuery
{
    // ========================================================================
    private string table = "";
    private List<string> output_fields = new List<string>();
    private List<string> conditions = new List<string>();
    private List<string> inner_joins = new List<string>();

    // ========================================================================
    public RawQuery(string table)
    {
        this.table = table;
    }

    // ========================================================================
    public void output(string table_field)
    {
        output_fields.Add(table_field);
    }

    // ------------------------------------------------------------------------
    public void output(string table, string table_field)
    {
        table_field = QueryUtils.bracket(table, table_field);
        output_fields.Add(table_field);
    }

    // ========================================================================
    public void where_(string table_field, int value)
    {
        conditions.Add($"{table_field} = {value}");
    }

    // ------------------------------------------------------------------------
    public void where_n(string table_field, string value)
    {
        conditions.Add($"{table_field} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(string table_field, string value)
    {
        conditions.Add($"{table_field} = '{value}'");
    }

    // ========================================================================
    public void where_(string table, string table_field, int value)
    {
        table_field = QueryUtils.bracket(table, table_field);
        conditions.Add($"{table_field} = {value}");
    }

    // ------------------------------------------------------------------------
    public void where_n(string table, string table_field, string value)
    {
        table_field = QueryUtils.bracket(table, table_field);
        conditions.Add($"{table_field} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(string table, string table_field, string value)
    {
        table_field = QueryUtils.bracket(table, table_field);
        conditions.Add($"{table_field} = '{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(string condition)
    {
        conditions.Add(condition);
    }

    // ========================================================================
    public void join(string table_name_1, string field_1, string table_name_2, string field_2)
    {
        inner_joins.Add(
            $"INNER JOIN {table_name_1} ON {table_name_1}.{field_1} = {table_name_2}.{field_2}"
        );
    }

    // ------------------------------------------------------------------------
    public void join(string table_name_1, string field_1, string field_2)
    {
        inner_joins.Add($"INNER JOIN {table_name_1} ON {field_1} = {field_2}");
    }

    // ------------------------------------------------------------------------
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
    public string get_select_query()
    {
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
        string query = $"SELECT {output_fields_str} FROM {table} ";
        query += string.Join(" ", inner_joins);
        query += get_where_clause() + ";";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_delete_query()
    {
        return $"DELETE FROM {table}" + get_where_clause() + ";";
    }

    // ------------------------------------------------------------------------
    public static string get_insert_query(string data, DatabaseTableConfig table_config)
    {
        string[] parts = data.Split(',');
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
    public string get_update_query(string data, DatabaseTableConfig table_config)
    {
        string[] parts = data.Split(',');
        string query = $"WHERE INTO {table_config.name} VALUES (";
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
        query += $" WHERE {get_where_clause()};";
        return query;
    }

    // ========================================================================
    public List<T> select<T>(SqlConnection conn)
        where T : DataObj, new() => Database.exec_query<T>(conn, get_select_query());

    // ------------------------------------------------------------------------
    public List<string> select(SqlConnection conn) => Database.exec_query(conn, get_select_query());

    // ------------------------------------------------------------------------
    public void select(SqlConnection conn, Database.ReaderFunction f) =>
        Database.exec_reader(conn, get_select_query(), f);

    // ------------------------------------------------------------------------
    public void delete(SqlConnection conn) => Database.exec_non_query(conn, get_delete_query());

    // ------------------------------------------------------------------------
    public static void insert(SqlConnection conn, string data, DatabaseTableConfig table_config) =>
        Database.exec_non_query(conn, get_insert_query(data, table_config));

    // ------------------------------------------------------------------------
    public void update(SqlConnection conn, string data, DatabaseTableConfig table_config) =>
        Database.exec_non_query(conn, get_update_query(data, table_config));

    // ========================================================================
}

/* EOF */
