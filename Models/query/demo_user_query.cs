namespace module_query;

using module_config;
using module_data;
using module_object;

sealed class DemoUserQuery
{
    // ========================================================================
    private static string get_table_name()
    {
        return Config.get_config("table", "demo_user");
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_all_demo_users()
    {
        string query = $"SELECT * FROM {get_table_name()}";
        return DataFetcher<DemoUser>.fetch(query);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        string query = $"SELECT * FROM {get_table_name()} WHERE id = {id}";
        return DataFetcher<DemoUser>.fetch(query);
    }

    // ========================================================================
}

/* EOF */
