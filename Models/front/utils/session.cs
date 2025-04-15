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
        user_name = "user_name",
        TRUE = "TRUE",
        FALSE = "FALSE";
}

static class SessionForm
{
    public static Dictionary<string, string> errors = new();
    public static bool displaying_error = false;
}

static class Session
{
    public static int? get_int(IQueryCollection query, string key)
    {
        string? value = query[key];
        if (value is null)
            return null;
        int result = 0;
        if (int.TryParse(value, out result))
            return result;
        else
            return null;
    }
}

static class SessionManager
{
    public static void log_in(ISession session, string table, int id)
    {
        switch (table)
        {
            case Tbl.student:
                session.SetString(SessionKey.user_role, SessionRole.student);
                break;

            case Tbl.teacher:
                session.SetString(SessionKey.user_role, SessionRole.teacher);
                break;

            case Tbl.admin:
                session.SetString(SessionKey.user_role, SessionRole.admin);
                break;
        }

        switch (table)
        {
            case Tbl.student
            or Tbl.teacher:
                Database.exec(
                    delegate(SqlConnection conn)
                    {
                        List<User> users = CommonQuery<User>.get_record_by_id(conn, table, id);
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
