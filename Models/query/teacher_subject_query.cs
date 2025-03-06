class TeacherSubjectQuery
{
    // ========================================================================
    public static void get_all_teacher_subjects(
        DatabaseConn.ReaderFunction f,
        string? conn_string = null
    )
    {
        Query q = new(Table.teacher_subject);
        q.select(f, conn_string);
    }
    // ========================================================================
}

/* EOF */
