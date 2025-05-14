using Microsoft.Data.SqlClient;

public class AdminMngCorSemCard
{
    public int TableIndex { get; set; }
    public int SemesterId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly FinishDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int NumParticipants { get; set; }
    public int Capacity { get; set; }
    public string Fee { get; set; } = string.Empty;

    public static Query GetQuery(int courseId)
    {
        Query q = new(Tbl.semester);
        q.Where(Field.semester__course_id, courseId);
        q.Output(Field.semester__id);
        q.Output(Field.semester__start_date);
        q.Output(Field.semester__finish_date);
        q.Output(Field.semester__status);
        q.Output(Field.semester__capacity);
        q.Output(Field.semester__fee);
        
        // Join with course to get course name
        q.Join(Field.course__id, Field.semester__course_id);
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        
        // Subquery to get number of participants
        Query participantsQuery = new(Tbl.request);
        participantsQuery.WhereField(Field.request__semester_id, Field.semester__id);
        participantsQuery.Output(QPiece.countAll);
        q.OutputQuery(participantsQuery.SelectQuery());
        
        // Order by start date descending (newest first)
        q.OrderBy(Field.semester__start_date, desc: true);
        
        return q;
    }

    public static AdminMngCorSemCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminMngCorSemCard card = new()
        {
            TableIndex = tableIndex++,
            SemesterId = QDataReader.GetInt(reader, ref pos),
            StartDate = QDataReader.GetDateOnly(reader, ref pos),
            FinishDate = QDataReader.GetDateOnly(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos),
            Capacity = QDataReader.GetInt(reader, ref pos),
            Fee = IoUtils.conv_fee(QDataReader.GetInt(reader, ref pos)),
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos),
            NumParticipants = QDataReader.GetInt(reader, ref pos)
        };
        return card;
    }
}
