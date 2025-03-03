sealed class DemoUserQuery
{
    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        string table_name = Config.get_config("tableNames", "demo_user");
        string query = $"SELECT * FROM {table_name}";
        return Database.fetch_data_new_conn<DemoUser>(query);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        return UserQuery.get_user_by_id<DemoUser>("demo_user", id);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_username_password(
        string username,
        string password
    )
    {
        return UserQuery.get_user_by_username_password<DemoUser>("demo_user", username, password);
    }

    // ========================================================================
}

/* EOF */
