using Microsoft.Data.SqlClient;

class ManageCourseCard
{
    public int table_index;
    public int course_id;
    public string course_name = "";
    public string course_state = "";
    public string avg_rating = "";
    public string subject = "";
    public int grade;

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.output(Field.course__id);
        q.output(Field.course__name);
        q.output(Field.course__status);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);

        string local_semester = "LocalSemester";
        string local_rating = "LocalRating";
        // rating avg
        Query q2 = new(Tbl.rating, local_rating);
        q2.joinAlias(Field.semester__id, local_semester, Field.rating__semester_id, local_rating);
        q2.WhereFieldAlias(Field.semester__course_id, local_semester, Field.course__id);
        q2.outputAvgCastFloat(Field.rating__stars, local_rating);
        q.outputQuery(q2.selectQuery());

        // rating count
        q2 = new(QPiece.alias(Tbl.rating, local_rating));
        q2.joinAlias(Field.semester__id, local_semester, Field.rating__semester_id, local_rating);
        q2.WhereFieldAlias(Field.semester__course_id, local_semester, Field.course__id);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.selectQuery());
        return q;
    }

    public static ManageCourseCard get_card(SqlDataReader reader, ref int current_table_index)
    {
        int pos = 0;
        int course_id = DataReader.getInt(reader, ref pos);
        string course_name = DataReader.getStr(reader, ref pos);
        string course_state = DataReader.getStr(reader, ref pos);
        var subject = DataReader.getStr(reader, ref pos);
        var grade = DataReader.getInt(reader, ref pos);

        var avg_rating = DataReader.getDouble(reader, ref pos);
        int num_ratings = DataReader.getInt(reader, ref pos);
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
        };

        return card;
    }
}
