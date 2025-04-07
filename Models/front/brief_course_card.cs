using System.Diagnostics;
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
        q.where_(
            Field.semester__state,
            [(int)InfoSemesterState.WAITING, (int)InfoSemesterState.STARTED]
        );
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

    static void get_card(SqlConnection conn, SqlDataReader reader, ref List<BriefCourseCard> cards)
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
        cards.Add(card);
    }

    public static List<BriefCourseCard> get_page(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20,
        string? search_by_course_name = null,
        string? search_by_teacher_name = null,
        string? search_by_subject_name = null
    )
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<BriefCourseCard> cards = new();
        Query q = get_query_creator();
        if (search_by_course_name != null)
        {
            q.where_string_contains(Field.course__name, search_by_course_name);
        }
        if (search_by_teacher_name != null)
        {
            q.where_string_contains(Field.teacher__name, search_by_teacher_name);
        }
        if (search_by_subject_name != null)
        {
            q.where_string_contains(Field.subject__name, search_by_subject_name);
        }
        q.offset(page, num_objs);
        q.select(conn, reader => get_card(conn, reader, ref cards));
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        Console.WriteLine($"get_brief_course_cards");
        Console.WriteLine($"Number of courses: {cards.Count}");
        Console.WriteLine($"Time taken: {elapsed.TotalMilliseconds} ms");
        return cards;
    }
}
