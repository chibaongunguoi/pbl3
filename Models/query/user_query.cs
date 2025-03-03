namespace module_query;

using module_config;
using module_data;
using module_object;

sealed class UserQuery
{
    // ========================================================================
    public static List<T> get_user_by_username_password<T>(
        string config_key,
        string username,
        string password
    )
        where T : DataObj, new()
    {
        var table_name = Config.get_config("tableNames", config_key);
        string query =
            $"SELECT * FROM {table_name} WHERE username = '{username}' AND password = '{password}'";
        return Database.fetch_data_new_conn<T>(query);
    }

    // ========================================================================
    public static List<T> get_user_by_id<T>(string config_key, int id)
        where T : DataObj, new()
    {
        var table_name = Config.get_config("tableNames", config_key);
        string query = $"SELECT * FROM {table_name} WHERE id = '{id}'";
        return Database.fetch_data_new_conn<T>(query);
    }

    // ========================================================================
    public string get_user_config_key(int id)
    {
        if (1000 <= id && id < 2000)
            return "student";
        else if (2000 <= id && id < 3000)
            return "teacher";
        return "";
    }

    // ========================================================================
    public string get_user_fullname(int id)
    {
        var user_config_key = get_user_config_key(id);
        if (user_config_key == "student")
        {
            List<Student> students = get_user_by_id<Student>(user_config_key, id);
            if (students.Count > 0)
                return students[0].fullname;
        }
        else if (user_config_key == "teacher")
        {
            List<Teacher> teachers = get_user_by_id<Teacher>(user_config_key, id);
            if (teachers.Count > 0)
                return teachers[0].fullname;
        }
        return "";
    }
}

/* EOF */
