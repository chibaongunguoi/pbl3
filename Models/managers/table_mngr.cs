using System.Text.Json;

class TableMngr
{
    // ========================================================================
    private static string s_database_name = "";
    private static Dictionary<string, DatabaseTableConfig> s_table_config_dict = new();

    private static Dictionary<string, string> s_table_name_dict = new();

    // ========================================================================
    public static void load(string table, DatabaseTableConfig table_config)
    {
        string table_name = table_config.name;

        s_table_name_dict[table] = table_name;
        s_table_config_dict[table] = table_config;
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
    public static string conv(string table) => s_table_name_dict[table];

    // ------------------------------------------------------------------------
    public static Dictionary<string, DatabaseTableConfig> get_table_configs() =>
        s_table_config_dict;

    // ------------------------------------------------------------------------
    public static DatabaseTableConfig get_table_config(string table) => s_table_config_dict[table];

    // ========================================================================
}

static class Tbl
{
    public const string none = "none";
    public const string admin = "TblAdmin";
    public const string student = "TblStudent";
    public const string teacher = "TblTeacher";
    public const string subject = "TblSubject";
    public const string course = "TblCourse";
    public const string semester = "TblSemester";
    public const string rating = "TblRating";
    public const string request = "TblRequest";
    public const string id_counter = "TblIdCounter";
}

static class Fld
{
    public const string id = "id";
    public const string username = "username";
    public const string password = "password";
    public const string name = "name";
    public const string gender = "gender";
    public const string bday = "bday";
    public const string thumbnail = "thumbnail";
    public const string description = "description";
    public const string grade = "grade";
    public const string state = "state";
    public const string date = "date";
    public const string start_date = "start_date";
    public const string finish_date = "finish_date";
    public const string capacity = "capacity";
    public const string fee = "fee";
    public const string tch_id = "tch_id";
    public const string sbj_id = "sbj_id";
    public const string stu_id = "stu_id";
    public const string course_id = "course_id";
    public const string semester_id = "semester_id";
    public const string stars = "stars";
    public const string count = "count";
    public const string min_count = "min_count";
    public const string max_count = "max_count";
}


/* EOF */
