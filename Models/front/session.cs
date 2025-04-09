enum SessionRole
{
    NONE,
    student,
    teacher,
    admin,
}

static class SessionKey
{
    public static string role = "role",
        user_id = "user_id";
}

static class SessionManager
{
    public static void log_in(ISession session, int user_id)
    {
        switch (AccountQuery<User>.get_latest_table())
        {
            case Table.student:
                session.SetInt32(SessionKey.role, (int)SessionRole.student);
                break;

            case Table.teacher:
                session.SetInt32(SessionKey.role, (int)SessionRole.teacher);
                break;

            case Table.admin:
                session.SetInt32(SessionKey.role, (int)SessionRole.admin);
                break;
        }

        session.SetInt32(SessionKey.user_id, user_id);
    }

    public static void log_out(ISession session)
    {
        session.Remove(SessionKey.user_id);
        session.Remove(SessionKey.role);
    }
}
