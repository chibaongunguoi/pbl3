sealed class DemoUserQuery
{
    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        return MultiAccountQueryFromTable<DemoUser>.get_all_accounts(
            ConfigUtils.s_demo_user_table_name
        );
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        return SingleAccountQueryFromTable.get_account_by_id<DemoUser>(
            ConfigUtils.s_demo_user_table_name,
            id
        );
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_username_password(
        string username,
        string password
    )
    {
        return SingleAccountQueryFromTable.get_account_by_username_password<DemoUser>(
            ConfigUtils.s_demo_user_table_name,
            username,
            password
        );
    }

    // ========================================================================
}

/* EOF */
