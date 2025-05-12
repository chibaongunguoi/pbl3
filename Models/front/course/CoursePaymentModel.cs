using Microsoft.AspNetCore.Mvc.ViewFeatures;
public class CoursePaymentModel
{
    public int courseId { get; set; }
    public int semesterId { get; set; }
    public int stuId { get; set; }
    public string teacherName { get; set; } = "";
    public string fee { get; set; } = "";
    public string description { get; set; } = "";
    public bool IsValid { get; set; } = false;
    public string? ErrorMessage { get; set; } = null;

    public CoursePaymentModel()
    { }

    public void Init(int courseId, string username)
    {
        this.courseId = courseId;
        Query q = new(Tbl.course);
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Join(Field.semester__course_id, Field.course__id);
        q.Where(Field.course__id, courseId);
        q.WhereQuery(Field.semester__id, SemesterQuery.GetLatestSemesterIdQuery("s"));
        q.Output(Field.teacher__name);
        q.Output(Field.semester__fee);
        q.Output(Field.course__name);
        q.Output(Field.semester__id);
        q.Output(Field.semester__capacity);
        q.Output(Field.semester__status);

        Query q4 = new(Tbl.request);
        q4.WhereField(Field.request__semester_id, Field.semester__id);
        q4.Output(QPiece.countAll);
        q.OutputQuery(q4.SelectQuery());

        Query q2 = new(Tbl.student);
        q2.Where(Field.student__username, username);
        q2.Output(Field.student__id);
        q2.Output(Field.student__name);

        string studentName = "";
        string courseName = "";
        string courseStatus = "";
        int myRequest = 0;
        int capacity = 0;
        int numParticipants = 0;

        Query q3 = new(Tbl.request);
        q3.Join(Field.student__id, Field.request__stu_id);
        q3.Join(Field.semester__id, Field.request__semester_id);
        q3.Join(Field.course__id, Field.semester__course_id);
        q3.WhereQuery(Field.semester__id, SemesterQuery.GetLatestSemesterIdQuery("s"));
        q3.Where(Field.student__username, username);
        q3.Where(Field.course__id, courseId);

        QDatabase.Exec(
            conn =>
            {
                q.Select(conn, reader =>
                {
                    int pos = 0;
                    teacherName = QDataReader.GetString(reader, ref pos);
                    var fee = QDataReader.GetInt(reader, ref pos);
                    this.fee = IoUtils.conv_fee(fee);
                    courseName = QDataReader.GetString(reader, ref pos);
                    semesterId = QDataReader.GetInt(reader, ref pos);
                    capacity = QDataReader.GetInt(reader, ref pos);
                    courseStatus = QDataReader.GetString(reader, ref pos);
                    numParticipants = QDataReader.GetInt(reader, ref pos);
                });

                q2.Select(conn, reader =>
                {
                    int pos = 0;
                    stuId = QDataReader.GetInt(reader, ref pos);
                    studentName = QDataReader.GetString(reader, ref pos);
                });

                myRequest = q3.Count(conn);
            }
        );
        description = $"{stuId} - {studentName} - {courseName}";
        IsValid = (myRequest == 0) && (numParticipants < capacity) && new List<string> { CourseStatus.started, CourseStatus.waiting }.Contains(courseStatus);

        if (myRequest > 0)
        {
            ErrorMessage = "Bạn đã tham gia khóa học này.";
        }
        else if (numParticipants >= capacity)
        {
            ErrorMessage = $"Khóa học đã đủ số lượng học viên ({numParticipants}/{capacity}).";
        }
        else if (courseStatus == CourseStatus.finished)
        {
            ErrorMessage = "Khóa học đã kết thúc.";
        }
    }
}