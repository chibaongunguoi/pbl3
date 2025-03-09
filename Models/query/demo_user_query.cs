sealed class DemoUserQuery
{
    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        return CommonQuery<DemoUser>.get_all_records(Table.demo_user);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(int id)
    {
        Query q = new(Table.demo_user);
        q.where_(Field.demo_user__id, id);
        return q.select<DemoUser>();
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_username_password(
        string username,
        string password
    )
    {
        return AccountQuery<DemoUser>.get_account_by_username_password(
            username,
            password,
            Table.demo_user
        );
    }

    // ========================================================================
}

/* EOF */
