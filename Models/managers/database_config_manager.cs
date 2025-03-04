using System.Text.Json;

class DatabaseConfigManager
{
    // ========================================================================
    private static DatabaseConfig s_database_config = new();

    // ========================================================================
    private static Dictionary<InfoAccountType, DatabaseTableConfig> s_account_table_config_dict =
        new();

    // ========================================================================
    public static void load()
    {
        string database_config_json = File.ReadAllText("database.json");
        s_database_config =
            JsonSerializer.Deserialize<DatabaseConfig>(database_config_json) ?? new();

        s_account_table_config_dict[InfoAccountType.STUDENT] = s_database_config.tables["student"];
        s_account_table_config_dict[InfoAccountType.TEACHER] = s_database_config.tables["teacher"];
    }

    // ========================================================================
    public static string get_database_name() => s_database_config.database_name;

    // ------------------------------------------------------------------------
    public static Dictionary<string, DatabaseTableConfig> get_table_configs() =>
        s_database_config.tables;

    // ========================================================================
    public static ref Dictionary<
        InfoAccountType,
        DatabaseTableConfig
    > get_account_table_config_dict() => ref s_account_table_config_dict;

    // ========================================================================
    public static DatabaseTableConfig get_account_table_config(InfoAccountType account_type)
    {
        if (!s_account_table_config_dict.ContainsKey(account_type))
            return new();
        return s_account_table_config_dict[account_type];
    }

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_demo_user_table_config() =>
        s_database_config.tables["demo_user"];

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_subject_table_config() =>
        s_database_config.tables["subject"];

    public static DatabaseTableConfig get_teacher_subject_table_config() =>
        s_database_config.tables["teacher_subject"];

    public static DatabaseTableConfig get_teacher_schedule_table_config() =>
        s_database_config.tables["teacher_schedule"];

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_contract_table_config() =>
        s_database_config.tables["contract"];

    public static DatabaseTableConfig get_contract_schedule_table_config() =>
        s_database_config.tables["contract_schedule"];

    // ========================================================================
}

/* EOF */
