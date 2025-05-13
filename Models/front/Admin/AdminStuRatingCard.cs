using Microsoft.Data.SqlClient;

public class AdminStuRatingCard
{
    public int TableIndex { get; set; }
    public int RatingId { get; set; }
    public int SemesterId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime RatingDate { get; set; }
    public int StuId { get; set; }
    public string StudentName { get; set; } = string.Empty;    public static Query GetQuery(int stuId)
    {
        Query q = new(Tbl.rating);

        // Join with necessary tables
        q.Join(Field.semester__id, Field.rating__semester_id);
        q.Join(Field.course__id, Field.semester__course_id);
        q.Join(Field.student__id, Field.rating__stu_id);

        // Filter by student ID
        q.Where(Field.student__id, stuId);

        // Output necessary fields
        // Note: There's no rating__id field available, so we'll use a combination of fields instead
        q.Output(Field.rating__stu_id); // stu_id + semester_id can identify a rating
        q.Output(Field.rating__semester_id);
        q.Output(Field.course__name);
        q.Output(Field.rating__stars);
        q.Output(Field.rating__description); // This is for comments, not rating__comment
        q.Output(Field.rating__timestamp); // This is for created date, not rating__created_at
        q.Output(Field.student__id);
        q.Output(Field.student__name);

        return q;
    }    public static AdminStuRatingCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminStuRatingCard card = new()
        {
            TableIndex = tableIndex++,
            // We don't have a proper RatingId field, so we'll use student ID as a placeholder
            // In a real app, you might want to create a composite key or handle this differently
            StuId = QDataReader.GetInt(reader, ref pos), // This is rating__stu_id
            SemesterId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos),
            Stars = QDataReader.GetInt(reader, ref pos),
            Comment = QDataReader.GetString(reader, ref pos), // This is from rating__description
            RatingDate = QDataReader.GetDateTime(reader, ref pos), // This is from rating__timestamp
            // We already got StuId above, but we need to skip this value in the reader
            RatingId = QDataReader.GetInt(reader, ref pos), // This is actually another copy of student__id
            StudentName = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}
