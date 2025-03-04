using Microsoft.Data.SqlClient;

sealed class QueryCreator
{
    // ========================================================================
    private string table_name = "";
    private List<string> output_fields = new List<string>();
    private List<string> conditions = new List<string>();
    private List<string> inner_joins = new List<string>();

    // ========================================================================
    public QueryCreator(string table_name)
    {
        this.table_name = table_name;
    }

    // ========================================================================
    public void add_output_field(string field_name)
    {
        output_fields.Add(field_name);
    }

    // ------------------------------------------------------------------------
    public void add_output_field(string table_name, string field_name)
    {
        output_fields.Add("[" + table_name + "].[" + field_name + "]");
    }

    // ========================================================================
    public void add_numeric_condition(string field_name, int value)
    {
        conditions.Add($"{field_name} = {value}");
    }

    // ------------------------------------------------------------------------
    public void add_nstring_condition(string field_name, string value)
    {
        conditions.Add($"{field_name} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void add_string_condition(string field_name, string value)
    {
        conditions.Add($"{field_name} = '{value}'");
    }

    // ------------------------------------------------------------------------
    public void add_inner_join(
        string table_name_1,
        string field_1,
        string table_name_2,
        string field_2
    )
    {
        inner_joins.Add(
            $"INNER JOIN {table_name_1} ON {table_name_1}.{field_1} = {table_name_2}.{field_2}"
        );
    }

    // ------------------------------------------------------------------------
    public void add_inner_join(string table_name_1, string field_1, string field_2)
    {
        inner_joins.Add($"INNER JOIN {table_name_1} ON {field_1} = {field_2}");
    }

    // ========================================================================
    public string get_filter_string()
    {
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
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
        string query = $"SELECT {output_fields_str} FROM {table_name} ";
        query += string.Join(" ", inner_joins);
        query += get_filter_string() + ";";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_delete_query()
    {
        return $"DELETE FROM {table_name}" + get_filter_string() + ";";
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

    // ========================================================================
    public List<T> exec_select_query<T>(SqlConnection conn)
        where T : DataObj, new() => DatabaseConn.exec_query<T>(conn, get_select_query());

    // ------------------------------------------------------------------------
    public List<T> exec_select_query<T>(string? conn_string = null)
        where T : DataObj, new() => Database.exec_query<T>(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public List<string> exec_select_query(SqlConnection conn) =>
        DatabaseConn.exec_query(conn, get_select_query());

    // ------------------------------------------------------------------------
    public List<string> exec_select_query(string? conn_string = null) =>
        Database.exec_query(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public void exec_select_query(SqlConnection conn, DatabaseConn.ReaderFunction f) =>
        DatabaseConn.exec_function(conn, get_select_query(), f);

    // ------------------------------------------------------------------------
    public void exec_select_query(DatabaseConn.ReaderFunction f, string? conn_string = null) =>
        Database.exec_function(get_select_query(), f, conn_string);

    // ========================================================================
    public void exec_delete_query(SqlConnection conn) =>
        DatabaseConn.exec_non_query(conn, get_delete_query());

    // ------------------------------------------------------------------------
    public void exec_delete_query(string? conn_string = null) =>
        Database.exec_non_query(get_delete_query(), conn_string);

    // ========================================================================
    public static void exec_insert_query(
        SqlConnection conn,
        string data,
        DatabaseTableConfig table_config
    ) => DatabaseConn.exec_non_query(conn, get_insert_query(data, table_config));

    // ------------------------------------------------------------------------
    public void exec_insert_query(
        string data,
        DatabaseTableConfig table_config,
        string? conn_string = null
    ) => Database.exec_non_query(get_insert_query(data, table_config), conn_string);

    // ========================================================================
}

/* EOF */
