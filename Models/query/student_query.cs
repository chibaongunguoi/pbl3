namespace module_query;

using module_object;

sealed class StudentQuery
{
    // ========================================================================
    public static List<Student> get_student_by_username_password(string username, string password)
    {
        return UserQuery.get_user_by_username_password<Student>("student", username, password);
    }

    // ========================================================================
}

/* EOF */
