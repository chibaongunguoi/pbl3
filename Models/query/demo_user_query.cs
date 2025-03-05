sealed class DemoUserQuery
{
    // ========================================================================
    public static List<DemoUser> get_all_demo_users()
    {
        Query q = new(Table.demo_user);
        return q.select<DemoUser>();
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
        Query q = new(Table.demo_user);
        q.where_(Field.demo_user__username, username);
        q.where_(Field.demo_user__password, password);
        return q.select<DemoUser>();
    }

    // ========================================================================
}

/* EOF */
