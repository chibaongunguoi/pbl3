namespace module_query;

using module_config;
using module_data;
using module_object;

sealed class UserQuery
{
    // ========================================================================
    private static string get_table_name()
    {
        return Config.get_config("tableNames", "user");
    }

    // ========================================================================
    public static List<User> get_all_demo_users()
    {
        string query = $"SELECT * FROM {UserQuery.get_table_name()}";
        return Database.fetch_data_new_conn<User>(query);
    }

    // ------------------------------------------------------------------------
    public static List<User> get_demo_user_by_id(int id)
    {
        string query = $"SELECT * FROM {UserQuery.get_table_name()} WHERE id = {id}";
        return Database.fetch_data_new_conn<User>(query);
    }

    // ========================================================================
}

/* EOF */
