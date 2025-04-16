using Microsoft.Data.SqlClient;

class BriefCourseCard : DataObj
{
    public int course_id;
    public string course_name = "";
    public string tch_name = "";
    public string subject = "";
    public int grade;
    public string dates = "";
    public string participants = "";
    public string avg_rating = "";
    public int num_ratings;
    public string fee = "";

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.join(Field.teacher__id, Field.course__tch_id);
        q.join(Field.semester__course_id, Field.course__id);
        q.Where(Field.semester__state, [SemesterState.waiting, SemesterState.started]);
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

        string local_alias = "local_alias";
        // rating avg
        Query q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.outputAvgCastFloat(Fld.stars);
        q.outputQuery(q2.selectQuery());

        // rating count
        q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.selectQuery());

        // participants count
        q2 = new(QPiece.alias(Tbl.request, local_alias));
        q2.Where(local_alias, Fld.semester_id, Tbl.semester, Fld.id);
        q2.Where(local_alias, Fld.state, RequestState.joined);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.selectQuery());

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

        this.course_id = course_id;
        this.course_name = course_name;
        this.tch_name = tch_name;
        this.subject = subject;
        this.grade = grade;
        this.dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}";
        this.participants = $"{num_participants}/{capacity}";
        this.avg_rating = $"{Math.Round(avg_rating, 1)}";
        this.num_ratings = num_ratings;
        this.fee = IoUtils.conv_fee(fee);
    }
}
