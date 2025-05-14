using Microsoft.Data.SqlClient;

public class AdminMngCorCard
{
    public int TableIndex { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int Grade { get; set; }

    public static Query GetQuery()
    {
        // Base query to get course information
        Query q = new(Tbl.course);
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        
        // Join teacher table to get teacher information
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Output(Field.teacher__name);
        q.Output(Field.teacher__id);
        
        // Join subject table to get subject information
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Output(Field.subject__name);
        q.Output(Field.subject__grade);
        
        // Subquery to get average rating
        Query avgRatingQuery = new(Tbl.rating);
        avgRatingQuery.Join(Field.semester__id, Field.rating__semester_id);
        avgRatingQuery.WhereField(Field.semester__course_id, Field.course__id);
        avgRatingQuery.OutputAvgCastFloat(Field.rating__stars);
        q.OutputQuery(avgRatingQuery.SelectQuery());
        
        // Subquery to get rating count
        Query ratingCountQuery = new(Tbl.rating);
        ratingCountQuery.Join(Field.semester__id, Field.rating__semester_id);
        ratingCountQuery.WhereField(Field.semester__course_id, Field.course__id);
        ratingCountQuery.Output(QPiece.countAll);
        q.OutputQuery(ratingCountQuery.SelectQuery());
        
        // Get the status of the latest semester
        Query statusQuery = new(Tbl.semester);
        statusQuery.WhereField(Field.semester__course_id, Field.course__id);
        statusQuery.OrderBy(Field.semester__start_date, desc: true);
        statusQuery.Offset(1, 1);
        statusQuery.Output(Field.semester__status);
        q.OutputQuery(statusQuery.SelectQuery());
        
        return q;
    }

    public static AdminMngCorCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminMngCorCard card = new()
        {
            TableIndex = tableIndex++,
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos),
            TeacherName = QDataReader.GetString(reader, ref pos),
            TeacherId = QDataReader.GetInt(reader, ref pos),
            Subject = QDataReader.GetString(reader, ref pos),
            Grade = QDataReader.GetInt(reader, ref pos),
            Rating = QDataReader.GetDouble(reader, ref pos),
            RatingCount = QDataReader.GetInt(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}
