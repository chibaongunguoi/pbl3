using Microsoft.Data.SqlClient;

class BriefCourseCard
{
    public int courseId;
    public string courseName = "";
    public string tchName = "";
    public string subject = "";
    public int grade;
    public string dates = "";
    public string participants = "";
    public string avgRating = "";
    public int numRatings;
    public string fee = "";
    public string MCourseStatus = "";
    public bool CanJoin { get; set; } = true;

    public static Query GetQueryCreator(string? role = null, string? username = null)
    {
        Query q = new(Tbl.course);
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Join(Field.semester__course_id, Field.course__id);
        q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
        q.Output(Field.course__id);
        q.Output(Field.semester__id);
        q.Output(Field.course__name);
        q.Output(Field.teacher__name);
        q.Output(Field.subject__name);
        q.Output(Field.subject__grade);
        q.Output(Field.semester__start_date);
        q.Output(Field.semester__finish_date);
        q.Output(Field.semester__capacity);
        q.Output(Field.semester__fee);
        q.Output(Field.course__status);


        string local_semester = "LocalSemester";
        string local_rating = "LocalRating";
        string local_request = "LocalRequest";
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

        // participants count
        q2 = new(Tbl.request, local_request);
        q2.WhereField(Field.request__semester_id, Field.semester__id, local_request);
        // q2.Where(Field.request__status, RequestStatus.joined, local_request);
        q2.Output(QPiece.countAll);
        q.OutputQuery(q2.SelectQuery());

        if (role == UserRole.Student && username is not null)
        {
            q2 = new(Tbl.request, local_request);
            q2.Join(Field.student__id, Field.request__stu_id, null, local_request);
            q2.WhereField(Field.request__semester_id, Field.semester__id, local_request);
            q2.WhereField(Field.student__username, username);
            q2.Output(QPiece.countAll);
            q.OutputQuery(q2.SelectQuery());
        }

        return q;
    }

    public static BriefCourseCard GetCard(SqlDataReader reader, ref int pos, string? role=null)
    {
        BriefCourseCard card = new();
        pos = 0;
        card.courseId = QDataReader.GetInt(reader, ref pos);
        int semester_id = QDataReader.GetInt(reader, ref pos);
        card.courseName = QDataReader.GetString(reader, ref pos);
        card.tchName = QDataReader.GetString(reader, ref pos);
        card.subject = QDataReader.GetString(reader, ref pos);
        card.grade = QDataReader.GetInt(reader, ref pos);
        var start_date = QDataReader.GetDateOnly(reader, ref pos);
        var finish_date = QDataReader.GetDateOnly(reader, ref pos);
        var capacity = QDataReader.GetInt(reader, ref pos);
        var fee = QDataReader.GetInt(reader, ref pos);
        card.MCourseStatus = QDataReader.GetString(reader, ref pos);
        var avg_rating = QDataReader.GetDouble(reader, ref pos);
        int num_ratings = QDataReader.GetInt(reader, ref pos);
        int num_participants = QDataReader.GetInt(reader, ref pos);

        card.dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}";
        card.participants = $"{num_participants}/{capacity}";
        card.avgRating = $"{Math.Round(avg_rating, 1)}";
        card.numRatings = num_ratings;
        card.fee = IoUtils.conv_fee(fee);


        if (role == UserRole.Student)
        {
            var requestCount = QDataReader.GetInt(reader, ref pos);
            card.CanJoin = (num_participants < capacity) && requestCount == 0 && new List<string> { CourseStatus.waiting, CourseStatus.started }.Contains(card.MCourseStatus);
        }
        else if (role == UserRole.Teacher)
        {
            card.CanJoin = false;
        }
        else
        {
            card.CanJoin = true;
        }

        return card;
    }
}
