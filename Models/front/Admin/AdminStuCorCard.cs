using Microsoft.Data.SqlClient;

public class AdminStuCorCard
{
    public int TableIndex { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int Grade { get; set; }
    public int Stars { get; set; }
    public int StuId { get; set; }
    public string StudentName { get; set; } = string.Empty;

    public static Query GetQuery(int stuId)
    {
        Query q = new(Tbl.course);

        // Join with semester, subject and request tables
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Join(Field.semester__course_id, Field.course__id);
        q.Join(Field.request__semester_id, Field.semester__id);
        q.Join(Field.student__id, Field.request__stu_id);

        // Filter by student ID
        q.Where(Field.student__id, stuId);

        // Output necessary fields
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        q.Output(Field.semester__status);
        q.Output(Field.subject__name);
        q.Output(Field.subject__grade);
        q.Output(Field.student__id);
        q.Output(Field.student__name);

        // Create a JoinQuery for the left join to get student's rating
        JoinQuery j = new(Tbl.rating, "R");
        j.AddField(Field.rating__semester_id, Field.semester__id);
        j.AddField(Field.rating__stu_id, Field.student__id);
        q.JoinClause(j.LeftJoin());
        
        // Output the student's rating (may be null if no rating)
        q.Output(Field.rating__stars, "R");

        return q;
    }

    public static AdminStuCorCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminStuCorCard card = new()
        {
            TableIndex = tableIndex++,
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos),
            Subject = QDataReader.GetString(reader, ref pos),
            Grade = QDataReader.GetInt(reader, ref pos),
            StuId = QDataReader.GetInt(reader, ref pos),
            StudentName = QDataReader.GetString(reader, ref pos),
            Stars = reader.IsDBNull(pos) ? 0 : QDataReader.GetInt(reader, ref pos)
        };
        return card;
    }
}