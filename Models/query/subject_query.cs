using Microsoft.Data.SqlClient;

class SubjectQuery
{
    // ========================================================================
    public static List<Subject> get_all_subjects(SqlConnection conn)
    {
        return CommonQuery<Subject>.get_all_records(conn, Table.subject);
    }

    // ------------------------------------------------------------------------
    public static List<Subject> get_subject_by_name(SqlConnection conn, string subject_name)
    {
        Query q = new(Table.subject);
        q.where_(Field.subject__name, subject_name);
        return q.select<Subject>(conn);
    }

    // ========================================================================
}

/* EOF */
