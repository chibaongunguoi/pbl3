using Microsoft.Data.SqlClient;

class TeacherSubjectQuery
{
    // ========================================================================
    public static void get_all_teacher_subjects(SqlConnection conn, Database.ReaderFunction f)
    {
        CommonQuery.get_all_records(conn, f, Table.teacher_subject);
    }
    // ========================================================================
}

/* EOF */
