using Microsoft.Data.SqlClient;

static class SessionRole
{
    public const string none = "",
        student = "student",
        teacher = "teacher",
        admin = "admin";
}

static class SessionKey
{
    public const string user_role = "user_role",
        user_id = "user_id",
        user_name = "user_name";
}

static class SessionManager
{
    public static void log_in(ISession session, Table table, int id)
    {
        switch (table)
        {
            case Table.student:
                session.SetString(SessionKey.user_role, SessionRole.student);
                break;

            case Table.teacher:
                session.SetString(SessionKey.user_role, SessionRole.teacher);
                break;

            case Table.admin:
                session.SetString(SessionKey.user_role, SessionRole.admin);
                break;
        }

        switch (table)
        {
            case Table.student
            or Table.teacher:
                Database.exec(
                    delegate(SqlConnection conn)
                    {
                        List<User> users = CommonQuery<User>.get_record_by_id(conn, id, table);
                        string name = users[0].name;
                        session.SetString(SessionKey.user_name, name);
                    }
                );
                break;
        }

        session.SetInt32(SessionKey.user_id, id);
    }

    public static void log_out(ISession session)
    {
        session.Remove(SessionKey.user_id);
        session.Remove(SessionKey.user_role);
    }
}
