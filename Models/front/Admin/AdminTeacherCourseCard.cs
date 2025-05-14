using Microsoft.Data.SqlClient;

public class AdminTeacherCourseCard
{
    public int TableIndex { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public string SubjectName { get; set; } = "";
    public int SemesterCount { get; set; }
    public double Rating { get; set; }
    public DateOnly? StartDate { get; set; }
    public string Status { get; set; } = "";

    public static Query GetQuery(int teacherId)
    {
        // Base query that gets course information for a specific teacher
        Query q = new(Tbl.course);
        q.Output(Field.course__id);
        q.Output(Field.course__name);
        q.Join(Field.subject__id, Field.course__sbj_id);
        q.Output(Field.subject__name);
        q.Where(Field.course__tch_id, teacherId);
        
        // Subquery to get semester count
        Query semesterCountQuery = new(Tbl.semester);
        semesterCountQuery.WhereField(Field.semester__course_id, Field.course__id);
        semesterCountQuery.Output(QPiece.countAll);
        q.OutputQuery(semesterCountQuery.SelectQuery());
        
        // Subquery to get average rating
        Query avgRatingQuery = new(Tbl.rating);
        avgRatingQuery.Join(Field.semester__id, Field.rating__semester_id);
        avgRatingQuery.Join(Field.course__id, Field.semester__course_id);
        avgRatingQuery.OutputAvgCastFloat(Field.rating__stars);
        q.OutputQuery(avgRatingQuery.SelectQuery());
        
        // Get the latest semester's start date
        Query startDateQuery = new(Tbl.semester);
        startDateQuery.WhereField(Field.semester__course_id, Field.course__id);
        startDateQuery.OrderBy(Field.semester__start_date, desc: true);
        startDateQuery.Offset(1, 1);
        startDateQuery.Output(Field.semester__start_date);
        q.OutputQuery(startDateQuery.SelectQuery());
        
        // Get the latest semester's status
        Query statusQuery = new(Tbl.semester);
        statusQuery.WhereField(Field.semester__course_id, Field.course__id);
        statusQuery.OrderBy(Field.semester__start_date, desc: true);
        statusQuery.Offset(1, 1);
        statusQuery.Output(Field.semester__status);
        q.OutputQuery(statusQuery.SelectQuery());
        return q;
    }

    public static AdminTeacherCourseCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminTeacherCourseCard card = new()
        {
            TableIndex = tableIndex++,
            CourseId = QDataReader.GetInt(reader, ref pos),
            CourseName = QDataReader.GetString(reader, ref pos),
            SubjectName = QDataReader.GetString(reader, ref pos),
            SemesterCount = QDataReader.GetInt(reader, ref pos),
            Rating = QDataReader.GetDouble(reader, ref pos),
            StartDate = QDataReader.GetDateOnly(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}
