class SubjectQuery
{
    // ========================================================================
    public static List<Subject> get_all_subjects()
    {
        Query q = new(Table.teacher_subject);
        return q.select<Subject>();
    }
    // ========================================================================
}

/* EOF */
