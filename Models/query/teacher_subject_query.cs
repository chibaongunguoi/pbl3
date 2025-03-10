using Microsoft.Data.SqlClient;

class TeacherSubjectQuery
{
    // ========================================================================
    public static List<string> get_all_teacher_subjects(SqlConnection conn)
    {
        return CommonQuery.get_all_records(conn, Table.teacher_subject);
    }
    // ========================================================================
}

/* EOF */
