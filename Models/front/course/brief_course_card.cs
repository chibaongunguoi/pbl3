using Microsoft.Data.SqlClient;

struct BriefCourseCard
{
    public int course_id;
    public string course_name;
    public string tch_name;
    public string subject;
    public int grade;
    public string dates;
    public string participants;
    public string avg_rating;
    public int num_ratings;
    public string fee;

    public static Query get_query_creator()
    {
        Query q = new Query(Table.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.join(Field.teacher__id, Field.course__tch_id);
        q.join(Field.semester__course_id, Field.course__id);
        q.where_str(Field.semester__state, [SemesterState.waiting, SemesterState.started]);
        q.output(Field.course__id);
        q.output(Field.semester__id);
        q.output(Field.course__name);
        q.output(Field.teacher__name);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);
        q.output(Field.semester__start_date);
        q.output(Field.semester__finish_date);
        q.output(Field.semester__capacity);
        q.output(Field.semester__fee);
        return q;
    }

    public static BriefCourseCard get_card(SqlConnection conn, SqlDataReader reader)
    {
        int pos = 0;
        int course_id = DataReader.get_int(reader, ref pos);

        int semester_id = DataReader.get_int(reader, ref pos);
        // Lấy sĩ số đã tham gia
        int num_participants = SemesterQuery.get_num_of_joined_requests(conn, semester_id);

        double avg_rating;
        int num_ratings;
        // Lấy trung bình và tổng số các đánh giá
        CourseQuery.get_avg_rating(conn, course_id, out avg_rating, out num_ratings);

        var course_name = DataReader.get_string(reader, ref pos);
        var tch_name = DataReader.get_string(reader, ref pos);
        var subject = DataReader.get_string(reader, ref pos);
        var grade = DataReader.get_int(reader, ref pos);
        var start_date = DataReader.get_date(reader, ref pos);
        var finish_date = DataReader.get_date(reader, ref pos);
        var capacity = DataReader.get_int(reader, ref pos);
        var fee = DataReader.get_int(reader, ref pos);

        BriefCourseCard card = new()
        {
            course_id = course_id,
            course_name = course_name,
            tch_name = tch_name,
            subject = subject,
            grade = grade,
            dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}",
            participants = $"{num_participants}/{capacity}",
            avg_rating = $"{Math.Round(avg_rating, 1)}",
            num_ratings = num_ratings,
            fee = IoUtils.conv_fee(fee),
        };
        return card;
    }
}
