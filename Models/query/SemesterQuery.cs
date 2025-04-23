static class SemesterQuery
{
    public static string getLatestSemesterIdQuery(
        string? semester_alias = null,
        string? course_alias = null
    )
    {
        Query q = new(Tbl.semester, semester_alias);
        q.WhereField(Field.semester__course_id, Field.course__id, semester_alias, course_alias);
        q.OrderBy(Field.semester__start_date, desc: true, semester_alias);
        q.outputTop(Field.semester__id, 1, semester_alias);
        return q.SelectQuery();
    }
}

/* EOF */
