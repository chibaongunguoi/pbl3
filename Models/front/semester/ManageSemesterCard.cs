using Microsoft.Data.SqlClient;

public class ManageSemesterCard
{
    public int CourseId;
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
        q.Output(Field.semester__course_id);
        q.Output(Field.semester__id);
        q.Output(Field.course__name);
        q.Output(Field.semester__start_date);
        q.Output(Field.semester__finish_date);
        q.Output(Field.semester__status);
        q.Output(Field.semester__fee);
        q.Output(Field.semester__capacity);

        Query q2 = new Query(Tbl.request);
        q2.WhereField(Field.request__semester_id, Field.semester__id);
        q2.Output(QPiece.countAll);
        q.OutputQuery(q2.SelectQuery());
        return q;
    }

    public static ManageSemesterCard get_card(SqlDataReader reader, ref int tableIdx)
    {
        int pos = 0;
        int course_id = QDataReader.GetInt(reader, ref pos);
        int semester_id = QDataReader.GetInt(reader, ref pos);
        string course_name = QDataReader.GetString(reader, ref pos);
        DateOnly start_date = QDataReader.GetDateOnly(reader, ref pos);
        DateOnly finish_date = QDataReader.GetDateOnly(reader, ref pos);
        string status = QDataReader.GetString(reader, ref pos);
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
        int fee = QDataReader.GetInt(reader, ref pos);
        int capacity = QDataReader.GetInt(reader, ref pos);
        int num_participants = QDataReader.GetInt(reader, ref pos);

        ManageSemesterCard card = new()
        {
            tableIdx = tableIdx++,
            CourseId = course_id,
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
