using Microsoft.Data.SqlClient;

sealed class DemoUserQuery
{
    // ========================================================================
    public static List<DemoUser> get_all_demo_users(SqlConnection conn)
    {
        return CommonQuery<DemoUser>.get_all_records(conn, Table.demo_user);
    }

    // ------------------------------------------------------------------------
    public static List<DemoUser> get_demo_user_by_id(SqlConnection conn, int id)
    {
        Query q = new(Table.demo_user);
        q.where_(Field.demo_user__id, id);
        return q.select<DemoUser>(conn);
    }

    // ========================================================================
}

/* EOF */
