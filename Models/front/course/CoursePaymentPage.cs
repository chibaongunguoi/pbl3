using Microsoft.Data.SqlClient;

class CoursePaymentPage
{
    public int courseId;
    public int semesterId;
    public int stuId;
    public string teacherName = "";
    public string fee = "";
    public string description = "";

    public CoursePaymentPage(int courseId, int stuId)
    {
        this.courseId = courseId;
        this.stuId = stuId;
        Query q = new(Tbl.course);
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Join(Field.semester__course_id, Field.course__id);
        q.Where(Field.course__id, courseId);
        q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
        q.Output(Field.teacher__name);
        q.Output(Field.semester__fee);
        q.Output(Field.course__name);
        q.Output(Field.semester__id);

        Query q2 = new(Tbl.student);
        q2.Where(Field.student__id, stuId);
        q2.Output(Field.student__name);

        string studentName = "";
        string courseName = "";

        void func(SqlConnection conn)
        {
            q.Select(conn, delegate (SqlDataReader reader)
            {
                int pos = 0;
                teacherName = QDataReader.GetString(reader, ref pos);
                var fee = QDataReader.GetInt(reader, ref pos);
                this.fee = IoUtils.conv_fee(fee);
                courseName = QDataReader.GetString(reader, ref pos);
                this.semesterId = QDataReader.GetInt(reader, ref pos);
            });

            q2.Select(conn, delegate (SqlDataReader reader)
            {
                studentName = QDataReader.GetString(reader);
            });
        }

        QDatabase.Exec(func);
        this.description = $"{stuId} - {studentName} - {courseName}";
    }

}