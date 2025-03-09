using System.Text.Json;

class TableMngr
{
    // ========================================================================
    private static string s_database_name = "";
    private static List<Table> s_account_tables = new() { Table.student, Table.teacher };
    private static Dictionary<Table, DatabaseTableConfig> s_table_config_dict = new();
    private static Dictionary<Field, string> s_table_field_name_dict = new();
    private static Dictionary<Table, string> s_table_name_dict = new();
    private static Dictionary<Field, Table> s_table_field_table_dict = new();

    // ========================================================================
    public static void load(string table_key, DatabaseTableConfig table_config)
    {
        string table_name = table_config.name;

        var table = Enum.Parse<Table>(table_key);
        s_table_name_dict[table] = table_name;
        s_table_config_dict[table] = table_config;

        foreach (var field in table_config.fields)
        {
            string table_field_name = $"{table_key}__{field.name}";
            Field field_name = Enum.Parse<Field>(table_field_name);
            s_table_field_name_dict[field_name] = QueryUtils.bracket(table_name, field.name);
            s_table_field_table_dict[field_name] = table;
        }
    }

    // ------------------------------------------------------------------------
    public static void load()
    {
        string database_config_json = File.ReadAllText("database.json");
        var db_config = JsonSerializer.Deserialize<DatabaseConfig>(database_config_json) ?? new();
        s_database_name = db_config.database_name;

        foreach (var (table_key, table_config) in db_config.tables)
        {
            load(table_key, table_config);
        }
    }

    // ========================================================================
    public static string get_database_name() => s_database_name;

    // ========================================================================
    public static string conv(Table table) => s_table_name_dict[table];

    // ------------------------------------------------------------------------
    public static string conv(Field table_field) => s_table_field_name_dict[table_field];

    // ------------------------------------------------------------------------
    public static Table get_table(Field table_field) => s_table_field_table_dict[table_field];

    // ------------------------------------------------------------------------
    public static Dictionary<Table, DatabaseTableConfig> get_table_configs() => s_table_config_dict;

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_table_config(Table table) => s_table_config_dict[table];

    // ------------------------------------------------------------------------
    public static List<Table> get_account_tables() => s_account_tables;

    // ========================================================================
}

/* EOF */
