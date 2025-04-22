using System.Text.Json;

// ============================================================================
public class DatabaseTableForeignKey
{
    public string field { get; set; } = "";
    public string ref_table { get; set; } = "";
    public string ref_field { get; set; } = "";
}

// ============================================================================
public class DatabaseTableField
{
    public string name { get; set; } = "";
    public string sql_type { get; set; } = "";
    public string dtype { get; set; } = "";
}

// ============================================================================
public class DatabaseTableConfig
{
    public string json_file { get; set; } = "";
    public List<DatabaseTableField> fields { get; set; } = new();
    public List<DatabaseTableForeignKey> foreign_keys { get; set; } = new();
}

// ============================================================================
public class DatabaseConfig
{
    public string database_name { get; set; } = "";
    public Dictionary<string, DatabaseTableConfig> tables { get; set; } = new();
}

public class TableMngr
{
    // ========================================================================
    private static string s_database_name = "";
    private static Dictionary<string, DatabaseTableConfig> s_table_config_dict = new();

    private static Dictionary<string, string> s_table_name_dict = new();

    // ========================================================================
    public static void load(string table, DatabaseTableConfig table_config)
    {
        s_table_config_dict[table] = table_config;
    }

    // ------------------------------------------------------------------------
    public static void load()
    {
        string database_config_json = File.ReadAllText("database.json");
        var db_config = JsonSerializer.Deserialize<DatabaseConfig>(database_config_json) ?? new();
        s_database_name = db_config.database_name;
        foreach (var (table, table_config) in db_config.tables)
        {
            load(table, table_config);
        }
    }

    // ========================================================================
    public static string get_database_name() => s_database_name;

    // ------------------------------------------------------------------------
    public static Dictionary<string, DatabaseTableConfig> get_table_configs() =>
        s_table_config_dict;

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_table_config(string table) => s_table_config_dict[table];

    // ========================================================================
}


/* EOF */
