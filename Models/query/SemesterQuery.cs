static class SemesterQuery
{
    public static string GetLatestSemesterIdQuery(
        string? semester_alias = null,
        string? course_alias = null
    )
    {
        Query q = new(Tbl.semester, semester_alias);
        q.WhereField(Field.semester__course_id, Field.course__id, semester_alias, course_alias);
        q.OrderBy(Field.semester__start_date, desc: true, semester_alias);
        q.OutputTop(Field.semester__id, 1, semester_alias);
        return q.SelectQuery();
    }    public static void GetRatingAvg(ref Query p, string? courseTableAlias = null)
    {
        string local_semester = "LocalSemester";
        string local_rating = "LocalRating";
        Query q = new(Tbl.rating, local_rating);
        q.Join(Field.semester__id, Field.rating__semester_id, local_semester, local_rating);
        q.WhereField(Field.semester__course_id, Field.course__id, local_semester, courseTableAlias);
        // Force the result to be a float by casting the ISNULL result as FLOAT
        q.OutputClause($"CAST(ISNULL(AVG(CAST([{local_rating}].[stars] AS FLOAT)), 0) AS FLOAT)");
        p.OutputQuery(q.SelectQuery(), "AVGRATING");
    }

    public static void GetRatingCount(ref Query p, string? courseTableAlias = null)
    {
        string local_semester = "LocalSemester";
        string local_rating = "LocalRating";
        Query q = new(Tbl.rating, local_rating);
        q.Join(Field.semester__id, Field.rating__semester_id, local_semester, local_rating);
        q.WhereField(Field.semester__course_id, Field.course__id, local_semester, courseTableAlias);
        q.Output(QPiece.countAll);
        p.OutputQuery(q.SelectQuery());
    }

    public static void GetParticipantsCount(ref Query p, string? semTableAlias = null)
    {
        string local_request = "LocalRequest";
        Query q = new(Tbl.request, local_request);
        q.WhereField(Field.request__semester_id, Field.semester__id, local_request, semTableAlias);
        q.Output(QPiece.countAll);
        p.OutputQuery(q.SelectQuery());
    }

    public static void GetStuRequestCount(ref Query p, string username, string? semTableAlias = null)
    {
        string local_request = "LocalRequest";
        string localStudent = "LocalStudent";
        Query q2 = new(Tbl.request, local_request);
        q2.Join(Field.student__id, Field.request__stu_id, localStudent, local_request);
        q2.WhereField(Field.request__semester_id, Field.semester__id, local_request);
        q2.WhereField(Field.student__username, username, localStudent);
        q2.Output(QPiece.countAll);
        p.OutputQuery(q2.SelectQuery());
    }
}

/* EOF */
