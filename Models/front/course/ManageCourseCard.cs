using Microsoft.Data.SqlClient;

class ManageCourseCard
{
    public int table_index;
    public int course_id { get; set; }
    public int semesterId = 0;
    public string course_name { get; set; } = "";
    public string course_state { get; set; } = "";
    public string avg_rating { get; set; } = "";
    public string subject { get; set; } = "";
    public int grade { get; set; }
    public int stars;
    public string comment = "";

    public static Query GetQueryCreator(bool stuMode =false)
    {
        Query q = new(Tbl.course);
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Join(Field.semester__course_id, Field.course__id);
        if (!stuMode)
            q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        q.Output(Field.semester__status);
        q.Output(Field.subject__name);
        q.Output(Field.subject__grade);
        q.Output(Field.semester__id);

        string local_semester = "LocalSemester"; // Alias
        string local_rating = "LocalRating";
        // rating avg
        Query q2 = new(Tbl.rating, local_rating);
        q2.Join(Field.semester__id, Field.rating__semester_id, local_semester, local_rating);
        q2.WhereField(Field.semester__course_id, Field.course__id, local_semester);
        q2.OutputAvgCastFloat(Field.rating__stars, local_rating);
        q.OutputQuery(q2.SelectQuery());

        // rating count
        q2 = new(Tbl.rating, local_rating);
        q2.Join(Field.semester__id, Field.rating__semester_id, local_semester, local_rating);
        q2.WhereField(Field.semester__course_id, Field.course__id, local_semester);
        q2.Output(QPiece.countAll);
        q.OutputQuery(q2.SelectQuery());
        return q;
    }

    public static ManageCourseCard GetCard(SqlDataReader reader, ref int pos, ref int current_table_index)
    {
        pos = 0;
        int course_id = QDataReader.GetInt(reader, ref pos);
        string course_name = QDataReader.GetString(reader, ref pos);
        string course_state = QDataReader.GetString(reader, ref pos);
        var subject = QDataReader.GetString(reader, ref pos);
        var grade = QDataReader.GetInt(reader, ref pos);
        int semesterId = QDataReader.GetInt(reader, ref pos);
        var avg_rating = QDataReader.GetDouble(reader, ref pos);
        int num_ratings = QDataReader.GetInt(reader, ref pos);

        string s_avg_rating = $"{Math.Round(avg_rating, 1)}/5 ({num_ratings})";

        string course_state_ui = "";
        switch (course_state)
        {
            case CourseStatus.waiting:
                course_state_ui = "Sắp diễn ra";
                break;
            case CourseStatus.started:
                course_state_ui = "Đang diễn ra";
                break;
            case CourseStatus.finished:
                course_state_ui = "Đã kết thúc";
                break;
        }

        ManageCourseCard card = new()
        {
            table_index = current_table_index++,
            course_id = course_id,
            course_name = course_name,
            course_state = course_state_ui,
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

        Query q2 = new(Tbl.request, "R");

        q2.OrderBy(Field.request__semester_id, desc: true, "R");
        q2.OutputTop(Field.request__semester_id, 1, "R");

        q2.Join(Field.student__id, Field.request__stu_id, "STU", "R");
        q2.WhereField(Field.student__username, username, "STU");

        q2.Join(Field.semester__id, Field.request__semester_id, "S", "R");
        q2.WhereField(Field.semester__course_id, Field.course__id, "S");

        q.WhereQuery(Field.semester__id, q2.SelectQuery());

        Query stuIdQ = new(Tbl.student, "STU");
        stuIdQ.Output(Field.student__id, "STU");
        stuIdQ.WhereField(Field.student__username, username, "STU");

        JoinQuery j = new(Tbl.rating);
        j.AddField(Field.rating__semester_id, Field.semester__id);
        j.AddQuery(Field.rating__stu_id, stuIdQ.SelectQuery());
        q.JoinClause(j.LeftJoin());

        q.Output(Field.rating__stars);
        q.Output(Field.rating__description);
        return q;
    }

    public static ManageCourseCard getStudentCourseCard(SqlDataReader reader, ref int pos, ref int current_table_index)
    {
        ManageCourseCard card = GetCard(reader, ref pos, ref current_table_index);
        card.stars = reader.IsDBNull(pos) ? 0 : QDataReader.GetInt(reader, ref pos);
        card.comment = reader.IsDBNull(pos) ? "" : QDataReader.GetString(reader, ref pos);
        return card;
    }
}
