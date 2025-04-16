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
        q.Join(Tbl.subject, Fld.id, Tbl.course, Fld.sbj_id);
        q.Join(Tbl.teacher, Fld.id, Tbl.course, Fld.tch_id);
        q.Join(Tbl.semester, Fld.course_id, Tbl.course, Fld.id);
        q.Where(Tbl.semester, Fld.state, [SemesterState.waiting, SemesterState.started]);
        q.Output(Tbl.course, Fld.id);
        q.Output(Tbl.semester, Fld.id);
        q.Output(Tbl.course, Fld.name);
        q.Output(Tbl.teacher, Fld.name);
        q.Output(Tbl.subject, Fld.name);
        q.Output(Tbl.subject, Fld.grade);
        q.Output(Tbl.semester, Fld.start_date);
        q.Output(Tbl.semester, Fld.finish_date);
        q.Output(Tbl.semester, Fld.capacity);
        q.Output(Tbl.semester, Fld.fee);

        string local_alias = "local_alias";
        Query q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.OutputClause(QPiece.avg(QPiece.cast_float(Fld.stars)));
        q.OutputClause(QPiece.bracket(q2.SelectQuery()));

        q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.OutputClause(QPiece.countAll);
        q.OutputClause(QPiece.bracket(q2.SelectQuery()));

        q2 = new(QPiece.alias(Tbl.request, local_alias));
        q2.Where(local_alias, Fld.semester_id, Tbl.semester, Fld.id);
        q2.Where(local_alias, Fld.state, RequestState.joined);
        q2.OutputClause(QPiece.countAll);
        q.OutputClause(QPiece.bracket(q2.SelectQuery()));

        return q;
    }

    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        int course_id = DataReader.get_int(reader, ref pos);
        int semester_id = DataReader.get_int(reader, ref pos);
        var course_name = DataReader.get_string(reader, ref pos);
        var tch_name = DataReader.get_string(reader, ref pos);
        var subject = DataReader.get_string(reader, ref pos);
        var grade = DataReader.get_int(reader, ref pos);
        var start_date = DataReader.get_date(reader, ref pos);
        var finish_date = DataReader.get_date(reader, ref pos);
        var capacity = DataReader.get_int(reader, ref pos);
        var fee = DataReader.get_int(reader, ref pos);
        var avg_rating = DataReader.get_double(reader, ref pos);
        int num_ratings = DataReader.get_int(reader, ref pos);
        int num_participants = DataReader.get_int(reader, ref pos);

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
