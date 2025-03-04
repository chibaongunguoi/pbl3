sealed class DemoUserQuery
{
    // ========================================================================
    public static string get_table_name()
    {
        return DatabaseConfigManager.get_demo_user_table_config().name;
    }

    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        return RecordQueryFromTable<DemoUser>.get_all_records(get_table_name());
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        return RecordQueryFromTable<DemoUser>.get_record_by_id(get_table_name(), id);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_username_password(
        string username,
        string password
    )
    {
        return AccountQueryFromTable<DemoUser>.get_account_by_username_password(
            get_table_name(),
            username,
            password
        );
    }

    // ========================================================================
}

/* EOF */
