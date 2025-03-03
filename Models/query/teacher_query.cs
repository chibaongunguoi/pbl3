namespace module_query;

using module_config;
using module_data;
using module_object;

sealed class TeacherQuery
{
    // ========================================================================
    private static string get_table_name()
    {
        return Config.get_config("tableNames", "student");
    }

    // ========================================================================
    public static List<Teacher> get_teacher_by_username_password(string username, string password)
    {
        string query =
            $"SELECT * FROM {get_table_name()} WHERE username = '{username}' AND password = '{password}'";

        return Database.fetch_data_new_conn<Teacher>(query);
    }

    // ========================================================================
}

/* EOF */
