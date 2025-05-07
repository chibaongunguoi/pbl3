using Microsoft.Data.SqlClient;

public class ManageRequestCard
{
    public int TableIndex { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentTel { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;

    public static Query GetQueryCreator()
    {
        Query q = new Query(Tbl.request);
        q.Join(Field.student__id, Field.request__stu_id);
        q.Join(Field.semester__id, Field.request__semester_id);
        q.Join(Field.course__id, Field.semester__course_id);
        q.Where(Field.request__status, RequestStatus.waiting);
        q.Output(Field.student__id);
        q.Output(Field.student__name);
        q.Output(Field.student__tel);
        q.Output(Field.semester__id);
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        return q;
    }
    public static ManageRequestCard GetCard(SqlDataReader reader, ref int tableIdx)
    {
        int pos = 0;
        ManageRequestCard card = new()
        {
            TableIndex = tableIdx++,
            StudentId = QDataReader.GetInt(reader, ref pos),
            StudentName = QDataReader.GetString(reader, ref pos),
            StudentTel = QDataReader.GetString(reader, ref pos),
            SemesterId = QDataReader.GetInt(reader, ref pos),
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}