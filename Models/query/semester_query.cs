static class SemesterQuery
{
    public static string get_latest_semester_id_query(
        string? semester_alias = null,
        string? course_alias = null
    )
    {
        Query q = new(Tbl.semester, semester_alias);
        q.WhereFieldAlias(
            Field.semester__course_id,
            semester_alias,
            Field.course__id,
            course_alias
        );
        q.orderByAlias(Field.semester__start_date, semester_alias, desc: true);
        q.outputTopAlias(Field.semester__id, semester_alias);
        return q.selectQuery();
    }
}

/* EOF */
