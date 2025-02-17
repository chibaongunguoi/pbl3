namespace module_query;

using module_config;
using module_data;
using module_object;

sealed class DemoUserQuery
{
    // ========================================================================
    private static string get_table_name()
    {
        return Config.get_config("tableNames", "demo_user");
    }

    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        string query = $"SELECT * FROM {get_table_name()}";
        return Database.fetch_data_new_conn<DemoUser>(query);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        string query = $"SELECT * FROM {get_table_name()} WHERE id = {id}";
        return Database.fetch_data_new_conn<DemoUser>(query);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_username_password(
        string username,
        string password
    )
    {
        string query =
            $"SELECT * FROM {get_table_name()} WHERE username = '{username}' AND password = '{password}'";

        return Database.fetch_data_new_conn<DemoUser>(query);
    }

    // ========================================================================
}

/* EOF */
