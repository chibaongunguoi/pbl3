using System.Text.Json;

enum Table
{
    demo_user,
    student,
    teacher,
    contract,
    subject,
    contract_schedule,
    teacher_subject,
    teacher_schedule,
}

enum Field
{
    demo_user__id,
    demo_user__username,
    demo_user__password,
    demo_user__name,
    demo_user__gender,
    demo_user__addr,
    demo_user__tel,
    demo_user__bday,
    demo_user__working_time,
    student__id,
    student__username,
    student__password,
    student__fullname,
    student__gender,
    student__addr,
    student__tel,
    student__bday,
    teacher__id,
    teacher__username,
    teacher__password,
    teacher__fullname,
    teacher__gender,
    teacher__addr,
    teacher__tel,
    teacher__bday,
    subject__id,
    subject__name,
    subject__grade,
    subject__duration,
    subject__fee_per_month,
    subject__slot_per_week,
    teacher_schedule__tch_id,
    teacher_schedule__interval_day,
    teacher_schedule__interval_start_time,
    teacher_schedule__interval_end_time,
    contract_schedule__ctrct_id,
    contract_schedule__interval_day,
    contract_schedule__interval_start_time,
    contract_schedule__interval_end_time,
    teacher_subject__tch_id,
    teacher_subject__sbj_id,
    contract__id,
    contract__stu_id,
    contract__tch_id,
    contract__sbj_id,
    contract__start_date,
}

class TableMngr
{
    // ========================================================================
    private static string s_database_name = "";
    private static List<Table> s_account_tables = new();
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
            s_table_field_name_dict[field_name] = QueryUtils.get_bracket_format(
                table_name,
                field.name
            );
            s_table_field_table_dict[field_name] = table;
        }
    }

    // ------------------------------------------------------------------------
    public static void load()
    {
        string database_config_json = File.ReadAllText("database.json");
        var db_config = JsonSerializer.Deserialize<DatabaseConfig>(database_config_json) ?? new();

        s_account_tables = new() { Table.student, Table.teacher };
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
    public static List<Table> get_account_tables() => s_account_tables;

    // ========================================================================
}

/* EOF */
