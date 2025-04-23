using Microsoft.Data.SqlClient;

class BriefCourseCard : DataObj
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

    public static Query getQueryCreator()
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
        return q;
    }

    public override void fetch(SqlDataReader reader, ref int pos)
    {
        int course_id = DataReader.getInt(reader, ref pos);
        int semester_id = DataReader.getInt(reader, ref pos);
        var course_name = DataReader.getStr(reader, ref pos);
        var tch_name = DataReader.getStr(reader, ref pos);
        var subject = DataReader.getStr(reader, ref pos);
        var grade = DataReader.getInt(reader, ref pos);
        var start_date = DataReader.getDate(reader, ref pos);
        var finish_date = DataReader.getDate(reader, ref pos);
        var capacity = DataReader.getInt(reader, ref pos);
        var fee = DataReader.getInt(reader, ref pos);
        var avg_rating = DataReader.getDouble(reader, ref pos);
        int num_ratings = DataReader.getInt(reader, ref pos);
        int num_participants = DataReader.getInt(reader, ref pos);

        this.courseId = course_id;
        this.courseName = course_name;
        this.tchName = tch_name;
        this.subject = subject;
        this.grade = grade;
        this.dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}";
        this.participants = $"{num_participants}/{capacity}";
        this.avgRating = $"{Math.Round(avg_rating, 1)}";
        this.numRatings = num_ratings;
        this.fee = IoUtils.conv_fee(fee);
    }
}
