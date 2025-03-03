sealed class TeacherQuery
{
    // ========================================================================
    public static List<Teacher> get_teacher_by_username_password(string username, string password)
    {
        return UserQuery.get_user_by_username_password<Teacher>("teacher", username, password);
    }

    // ========================================================================
}

/* EOF */
