using System.Text.Json;

class ConfigJsonTableField
{
    // ========================================================================
    public string name { get; set; } = "";
    public string sql_type { get; set; } = "";
    public string dtype { get; set; } = "";

    // ========================================================================
}

class ConfigJsonTable
{
    // ========================================================================
    public string name { get; set; } = "";
    public string csv_file { get; set; } = "";
    public List<ConfigJsonTableField> fields { get; set; } = new();

    // ========================================================================
}

class ConfigJsonDetails
{
    // ========================================================================
    public string database_name { get; set; } = "";
    public bool data_generator { get; set; } = false;
    public Dictionary<string, ConfigJsonTable> tables { get; set; } = new();

    // ========================================================================
}

class ConfigJson
{
    // ========================================================================
    private static ConfigJsonDetails s_config_json = new();
    private static string s_server_name = "";

    // ========================================================================
    public static void load()
    {
        string json = File.ReadAllText("config.json");
        s_server_name = File.ReadAllText("server.txt").Trim();
        s_config_json = JsonSerializer.Deserialize<ConfigJsonDetails>(json) ?? new();
    }

    // ========================================================================
    public static string get_table_name(string key) => s_config_json.tables[key].name;

    // ------------------------------------------------------------------------
    public static Dictionary<string, ConfigJsonTable> get_tables() => s_config_json.tables;

    // ------------------------------------------------------------------------
    public static string get_server_name() => s_server_name;

    // ------------------------------------------------------------------------
    public static string get_database_name() => s_config_json.database_name;

    //
    public static bool get_data_generator() => s_config_json.data_generator;

    // ========================================================================
};

/* EOF */
