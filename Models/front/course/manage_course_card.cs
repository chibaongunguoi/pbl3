using Microsoft.Data.SqlClient;

struct ManageCourseCard
{
    public int table_index;
    public int course_id;
    public string course_name;
    public string course_state;
    public string avg_rating;
    public string subject;
    public int grade;

    public static Query get_query_creator()
    {
        Query q = new Query(Table.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.output(Field.course__id);
        q.output(Field.course__name);
        q.output(Field.course__state);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);
        return q;
    }

    public static ManageCourseCard get_card(
        SqlConnection conn,
        SqlDataReader reader,
        ref int current_table_index
    )
    {
        int pos = 0;
        int course_id = DataReader.get_int(reader, ref pos);
        string course_name = DataReader.get_string(reader, ref pos);
        string course_state = DataReader.get_string(reader, ref pos);
        var subject = DataReader.get_string(reader, ref pos);
        var grade = DataReader.get_int(reader, ref pos);

        double avg_rating;
        int num_ratings;
        CourseQuery.get_avg_rating(conn, course_id, out avg_rating, out num_ratings);
        string s_avg_rating = $"{Math.Round(avg_rating, 1)}/5 ({num_ratings})";
        ManageCourseCard card = new()
        {
            table_index = current_table_index++,
            course_id = course_id,
            course_name = course_name,
            course_state = course_state == CourseState.waiting ? "Đang diễn ra" : "Đã kết thúc",
            subject = subject,
            grade = grade,
            avg_rating = s_avg_rating,
        };

        return card;
    }
}
