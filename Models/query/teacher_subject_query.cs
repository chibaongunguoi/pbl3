class TeacherSubjectQuery
{
    // ========================================================================
    public static void get_all_teacher_subjects(DatabaseConn.ReaderFunction f)
    {
        CommonQuery.get_all_records(f, Table.teacher_subject);
    }
    // ========================================================================
}

/* EOF */
