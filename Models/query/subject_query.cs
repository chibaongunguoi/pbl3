class SubjectQuery
{
    // ========================================================================
    public static List<Subject> get_all_subjects()
    {
        Query q = new(Table.subject);
        return q.select<Subject>();
    }

    // ------------------------------------------------------------------------
    public static List<Subject> get_subject_by_name(string subject_name)
    {
        Query q = new(Table.subject);
        q.where_(Field.subject__name, subject_name);
        return q.select<Subject>();
    }

    // ========================================================================
}

/* EOF */
