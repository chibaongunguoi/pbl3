namespace module_query;

using module_config;
using module_database;
using module_info;

class DemoUserQuery
{
    private static string get_table_name()
    {
        return Config.get_config("table", "demo_user");
    }

    public static List<DemoUser> get_all_demo_users()
    {
        string query = $"SELECT * FROM {get_table_name()}";
        return DatabaseReceiver<DemoUser>.get_query_results(query);
    }

    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        string query = $"SELECT * FROM {get_table_name()} WHERE id = {id}";
        return DatabaseReceiver<DemoUser>.get_query_results(query);
    }
}
