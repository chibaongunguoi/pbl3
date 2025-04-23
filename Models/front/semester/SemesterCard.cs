using Microsoft.Data.SqlClient;

class SemesterCard
{
    public int tableIdx;
    public int semesterId;
    public string courseName = "";
    public string startDate = "";
    public string finishDate = "";
    public string status = "";
    public string fee = "";
    public int capacity;
    public int numParticipants;

    public static Query get_query_creator()
    {
        Query q = new Query(Tbl.semester);
        q.Join(Field.course__id, Field.semester__course_id);
        q.output(Field.semester__id);
        q.output(Field.course__name);
        q.output(Field.semester__start_date);
        q.output(Field.semester__finish_date);
        q.output(Field.semester__status);
        q.output(Field.semester__fee);
        q.output(Field.semester__capacity);

        Query q2 = new Query(Tbl.request);
        q2.WhereField(Field.request__semester_id, Field.semester__id);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.SelectQuery());
        return q;
    }

    public static SemesterCard get_card(SqlDataReader reader, ref int tableIdx)
    {
        int pos = 0;
        int semester_id = DataReader.getInt(reader, ref pos);
        string course_name = DataReader.getStr(reader, ref pos);
        DateOnly start_date = DataReader.getDate(reader, ref pos);
        DateOnly finish_date = DataReader.getDate(reader, ref pos);
        string status = DataReader.getStr(reader, ref pos);
        switch (status)
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
        int fee = DataReader.getInt(reader, ref pos);
        int capacity = DataReader.getInt(reader, ref pos);
        int num_participants = DataReader.getInt(reader, ref pos);

        SemesterCard card = new()
        {
            tableIdx = tableIdx++,
            semesterId = semester_id,
            courseName = course_name,
            startDate = IoUtils.conv(start_date),
            finishDate = IoUtils.conv(finish_date),
            status = status,
            fee = IoUtils.conv_fee(fee),
            capacity = capacity,
            numParticipants = num_participants,
        };
        return card;
    }
}
