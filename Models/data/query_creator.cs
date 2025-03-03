sealed class QueryCreator<T>
    where T : DataObj, new()
{
    // ========================================================================
    private string table_name = "";
    private List<string> output_fields = new List<string>();
    private List<string> conditions = new List<string>();

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

    // ========================================================================
    public string get_query()
    {
        var output_fields_str = output_fields.Count > 0 ? string.Join(", ", output_fields) : "*";
        var conditions_str = string.Join(" AND ", conditions);
        var query = $"SELECT {output_fields_str} FROM {table_name}";
        if (conditions_str.Length > 0)
            query += $" WHERE {conditions_str}";
        query += ";";
        return query;
    }

    // ========================================================================
    public List<T> exec_query()
    {
        return Database.exec_query<T>(get_query());
    }
    // ========================================================================
}

/* EOF */
