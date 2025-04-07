using Microsoft.Data.SqlClient;

struct DetailedCourseCard
{
    public string course_name;
    public string tch_name;
    public string subject;
    public int grade;
    public string dates;
    public string participants;
    public string avg_rating;
    public int num_ratings;
    public string fee;
    public string description;

    static Query get_query_creator()
    {
        Query q = BriefCourseCard.get_query_creator();
        q.output(Field.course__description);
        return q;
    }

    static void get_card(
        SqlConnection conn,
        SqlDataReader reader,
        ref List<DetailedCourseCard> cards
    )
    {
        int pos = 0;
        int course_id = DataReader.get_int(reader, pos++);

        int semester_id = DataReader.get_int(reader, pos++);
        int num_participants = SemesterQuery.get_num_of_joined_requests(conn, semester_id);

        double avg_rating;
        int num_ratings;
        CourseQuery.get_avg_rating(conn, course_id, out avg_rating, out num_ratings);

        var course_name = DataReader.get_string(reader, pos++);
        var tch_name = DataReader.get_string(reader, pos++);
        var subject = DataReader.get_string(reader, pos++);
        var grade = DataReader.get_int(reader, pos++);
        var start_date = DataReader.get_data_obj<InfoDate>(reader, ref pos);
        var finish_date = DataReader.get_data_obj<InfoDate>(reader, ref pos);
        var capacity = DataReader.get_int(reader, pos++);
        var fee = DataReader.get_int(reader, pos++);
        var description = DataReader.get_string(reader, pos++);

        DetailedCourseCard card = new()
        {
            course_name = course_name,
            tch_name = tch_name,
            subject = subject,
            grade = grade,
            dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}",
            participants = $"{num_participants}/{capacity}",
            avg_rating = $"{Math.Round(avg_rating, 1)}",
            num_ratings = num_ratings,
            fee = IoUtils.conv_fee(fee),
            description = description,
        };
        cards.Add(card);
    }

    public static List<DetailedCourseCard> get_by_id(SqlConnection conn, int id)
    {
        List<DetailedCourseCard> cards = new();
        Query q = get_query_creator();
        q.where_(Field.course__id, id);
        q.select(conn, reader => get_card(conn, reader, ref cards));
        return cards;
    }
}
