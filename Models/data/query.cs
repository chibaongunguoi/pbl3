using Microsoft.Data.SqlClient;

sealed class Query
{
    // ========================================================================
    private Table table = new();
    private List<string> output_fields = new List<string>();
    private List<string> conditions = new List<string>();
    private List<string> inner_joins = new List<string>();

    // ========================================================================
    public Query(Table table)
    {
        this.table = table;
    }

    // ========================================================================
    private string conv(Table table) => DatabaseConfigManager.get_table_name(table);

    // ------------------------------------------------------------------------
    private string conv(Field table_field) =>
        DatabaseConfigManager.get_table_field_name(table_field);

    // ------------------------------------------------------------------------
    private string get_table(Field table_field) =>
        conv(DatabaseConfigManager.get_table_field_table(table_field));

    // ========================================================================
    public void output(Field table_field)
    {
        output_fields.Add(conv(table_field));
    }

    // ========================================================================
    public void where_(Field table_field, int value)
    {
        conditions.Add($"{conv(table_field)} = {value}");
    }

    // ------------------------------------------------------------------------
    public void where_n(Field table_field, string value)
    {
        conditions.Add($"{conv(table_field)} = N'{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(Field table_field, string value)
    {
        conditions.Add($"{conv(table_field)} = '{value}'");
    }

    // ------------------------------------------------------------------------
    public void where_(string condition)
    {
        conditions.Add(condition);
    }

    // ========================================================================

    // ------------------------------------------------------------------------
    public void join(Field field_1_, Field field_2_)
    {
        string table_name_1 = get_table(field_1_);
        var field_1 = conv(field_1_);
        var field_2 = conv(field_2_);
        inner_joins.Add($"INNER JOIN {table_name_1} ON {field_1} = {field_2}");
    }

    // ------------------------------------------------------------------------
    public void join(string join_cmd)
    {
        inner_joins.Add(join_cmd);
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
        var table_name = DatabaseConfigManager.get_table_name(table);
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
        string query = $"SELECT {output_fields_str} FROM {table_name} ";
        query += string.Join(" ", inner_joins);
        query += get_filter_string() + ";";
        return query;
    }

    // ------------------------------------------------------------------------
    public string get_delete_query()
    {
        var table_name = DatabaseConfigManager.get_table_name(table);
        return $"DELETE FROM {table_name}" + get_filter_string() + ";";
    }

    // ========================================================================
    public List<T> select<T>(SqlConnection conn)
        where T : DataObj, new() => DatabaseConn.exec_query<T>(conn, get_select_query());

    // ------------------------------------------------------------------------
    public List<string> select(SqlConnection conn) =>
        DatabaseConn.exec_query(conn, get_select_query());

    // ------------------------------------------------------------------------
    public void select(SqlConnection conn, DatabaseConn.ReaderFunction f) =>
        DatabaseConn.exec_reader_function(conn, get_select_query(), f);

    // ========================================================================
    public void delete(SqlConnection conn) => DatabaseConn.exec_non_query(conn, get_delete_query());

    // ========================================================================
    public List<T> select<T>(string? conn_string = null)
        where T : DataObj, new() => Database.exec_query<T>(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public List<string> select(string? conn_string = null) =>
        Database.exec_query(get_select_query(), conn_string);

    // ------------------------------------------------------------------------
    public void select(DatabaseConn.ReaderFunction f, string? conn_string = null) =>
        Database.exec_reader_function(get_select_query(), f, conn_string);

    // ========================================================================
    public void delete(string? conn_string = null) =>
        Database.exec_non_query(get_delete_query(), conn_string);

    // ========================================================================
}

/* EOF */
