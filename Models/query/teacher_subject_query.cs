class TeacherSubjectQuery
{
    // ========================================================================
    public static List<TeacherSubject> get_all_teacher_subjects()
    {
        Query q = new(Table.teacher_subject);
        return q.select<TeacherSubject>();
    }
    // ========================================================================
}

/* EOF */
