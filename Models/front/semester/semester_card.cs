using Microsoft.Data.SqlClient;


class SemesterCard
{
    public int semester_id { get; set; }
    public string course_name { get; set; } = "";
    public string start_date { get; set; } = "";
    public string finish_date { get; set; } = "";
    public string status { get; set; } = "";
    public string fee { get; set; } = "";
    public int capacity { get; set; }
    public int num_participants { get; set; }

    public static Query get_query_creator()
    {
        Query q = new Query(Tbl.semester);
        q.join(Field.course__id, Field.semester__course_id);
        q.output(Field.semester__id);
        q.output(Field.course__name);
        q.output(Field.semester__start_date);
        q.output(Field.semester__finish_date);
        q.output(Field.semester__status);
        q.output(Field.semester__fee);
        q.output(Field.semester__capacity);

        Query q2 = new Query(Tbl.request);
        q2.join(Field.semester__id, Field.request__semester_id);
        q2.Where(Field.request__status, RequestStatus.joined);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.selectQuery());
        return q;

    }

    public static SemesterCard get_card(SqlDataReader reader, ref int current_table_index)
    {
        int pos = 0;
        int semester_id = DataReader.getInt(reader, ref pos);
        string course_name = DataReader.getStr(reader, ref pos);
        DateOnly start_date = DataReader.getDate(reader, ref pos);
        DateOnly finish_date = DataReader.getDate(reader, ref pos);
        string status = DataReader.getStr(reader, ref pos);
        switch(status)
        {
            case SemesterStatus.waiting:
                status = "Sắp diễn ra";
                break;
            case SemesterStatus.started:
                status = "Đang diễn ra";
                break;
            case SemesterStatus.finished:
                status = "Đã kết thúc";
                break;
        }
        string fee = DataReader.getStr(reader, ref pos);
        int capacity = DataReader.getInt(reader, ref pos);
        int num_participants = DataReader.getInt(reader, ref pos);

        SemesterCard card = new()
        {
            semester_id = semester_id,
            course_name = course_name,
            start_date = IoUtils.conv(start_date),
            finish_date = IoUtils.conv(finish_date),
            status = status,
            fee = fee,
            capacity = capacity,
            num_participants = num_participants
        };
        return card;
    }


}