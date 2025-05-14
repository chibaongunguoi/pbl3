using Microsoft.Data.SqlClient;

public class AdminMngRatingCard
{
    public int TableIndex { get; set; }
    // Composite key of StudentId and SemesterId
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }    public static Query GetQuery()
    {
        Query q = new(Tbl.rating);
        q.Output(Field.rating__stu_id);
        q.Output(Field.rating__semester_id);
        q.Output(Field.rating__timestamp);
        q.Output(Field.rating__stars);
        q.Output(Field.rating__description);
        
        q.Join(Field.student__id, Field.rating__stu_id);
        q.Output(Field.student__name);
        
        q.Join(Field.semester__id, Field.rating__semester_id);
        q.Output(Field.semester__course_id);
        
        q.Join(Field.course__id, Field.semester__course_id);
        q.Output(Field.course__name);
        
        q.OrderBy(Field.rating__timestamp, desc: true);
        
        return q;
    }    public static AdminMngRatingCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminMngRatingCard card = new()
        {
            TableIndex = tableIndex++,
            StudentId = QDataReader.GetInt(reader, ref pos),
            SemesterId = QDataReader.GetInt(reader, ref pos),
            Timestamp = QDataReader.GetDateTime(reader, ref pos),
            Stars = QDataReader.GetInt(reader, ref pos),
            Description = QDataReader.GetString(reader, ref pos),
            StudentName = QDataReader.GetString(reader, ref pos),
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}
