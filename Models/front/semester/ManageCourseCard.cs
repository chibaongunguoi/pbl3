using Microsoft.Data.SqlClient;

class ManageCourseCard
{
    public int table_index;
    public int course_id { get; set; }
    public int semesterId = 0;
    public string course_name { get; set; } = "";
    public int TchId { get; set; }
    public string TeacherName { get; set; } = "";
    public string MSemesterStatus { get; set; } = "";
    public string avg_rating { get; set; } = "";
    public string subject { get; set; } = "";
    public int grade { get; set; }
    public int stars;
    public string comment = "";
    public string? MRequestStatus { get; set; } = null;

    public bool Commentable = false;

    public static Query GetQueryCreator(bool stuMode = false)
    {
        Query q = new(Tbl.course);
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Join(Field.semester__course_id, Field.course__id);
        q.Join(Field.teacher__id, Field.course__tch_id);
        if (!stuMode)
        {
            q.WhereQuery(Field.semester__id, SemesterQuery.GetLatestSemesterIdQuery("s"));
        }
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        q.Output(Field.teacher__id);
        q.Output(Field.teacher__name);
        q.Output(Field.semester__status);
        q.Output(Field.subject__name);
        q.Output(Field.subject__grade);
        q.Output(Field.semester__id);

        SemesterQuery.GetRatingAvg(ref q);
        SemesterQuery.GetRatingCount(ref q);
        return q;
    }

    public static ManageCourseCard GetCard(SqlDataReader reader, ref int pos, ref int current_table_index)
    {
        pos = 0;
        int course_id = QDataReader.GetInt(reader, ref pos);
        string course_name = QDataReader.GetString(reader, ref pos);
        int TchId = QDataReader.GetInt(reader, ref pos);
        string teacher_name = QDataReader.GetString(reader, ref pos);
        string semesterStatus = QDataReader.GetString(reader, ref pos);
        var subject = QDataReader.GetString(reader, ref pos);
        var grade = QDataReader.GetInt(reader, ref pos);
        int semesterId = QDataReader.GetInt(reader, ref pos);
        var avg_rating = QDataReader.GetDouble(reader, ref pos);
        int num_ratings = QDataReader.GetInt(reader, ref pos);
        string s_avg_rating = $"{Math.Round(avg_rating, 1)}/5 ({num_ratings})";

        ManageCourseCard card = new()
        {
            table_index = current_table_index++,
            course_id = course_id,
            course_name = course_name,
            TchId = TchId,
            TeacherName = teacher_name,
            MSemesterStatus = semesterStatus,
            subject = subject,
            grade = grade,
            avg_rating = s_avg_rating,
            semesterId = semesterId
        };

        return card;
    }

    public static Query GetStudentCourseQueryCreator(string username)
    {
        Query q = GetQueryCreator(stuMode: true);

        Query semIdQ = new(Tbl.request, "R");
        semIdQ.OrderBy(Field.request__semester_id, desc: true, "R");
        semIdQ.Join(Field.student__id, Field.request__stu_id, "STU", "R");
        semIdQ.Where(Field.student__username, username, "STU");
        semIdQ.Join(Field.semester__id, Field.request__semester_id, "S", "R");
        semIdQ.WhereField(Field.semester__course_id, Field.course__id, "S");
        semIdQ.OutputTop(Field.semester__id, 1, "S");
        q.WhereQuery(Field.semester__id, semIdQ.SelectQuery());

        Query stuIdQ = new(Tbl.student, "stuIdQ");
        stuIdQ.Output(Field.student__id, "stuIdQ");
        stuIdQ.Where(Field.student__username, username, "stuIdQ");

        JoinQuery j = new(Tbl.rating);
        j.AddField(Field.rating__semester_id, Field.semester__id);
        j.AddQuery(Field.rating__stu_id, stuIdQ.SelectQuery());
        q.JoinClause(j.LeftJoin());

        j = new(Tbl.request);
        j.AddField(Field.request__semester_id, Field.semester__id);
        j.AddQuery(Field.request__stu_id, stuIdQ.SelectQuery());
        q.JoinClause(j.LeftJoin());

        q.Output(Field.request__status);
        q.Output(Field.rating__stars);
        q.Output(Field.rating__description);
        return q;
    }

    public static ManageCourseCard getStudentCourseCard(SqlDataReader reader, ref int pos, ref int current_table_index)
    {
        ManageCourseCard card = GetCard(reader, ref pos, ref current_table_index);
        card.MRequestStatus = reader.IsDBNull(pos) ? null : QDataReader.GetString(reader, ref pos);
        card.stars = reader.IsDBNull(pos) ? 0 : QDataReader.GetInt(reader, ref pos);
        card.comment = reader.IsDBNull(pos) ? "" : QDataReader.GetString(reader, ref pos);
        card.avg_rating = card.stars > 0 ? $"{card.stars} / 5" : "Chưa đánh giá";
        card.Commentable = new List<string> { SemesterStatus.finished }.Contains(card.MSemesterStatus) && card.MRequestStatus == RequestStatus.joined;
        return card;
    }
}