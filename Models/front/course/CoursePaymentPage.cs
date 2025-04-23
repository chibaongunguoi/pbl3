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
        q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
        q.output(Field.teacher__name);
        q.output(Field.semester__fee);
        q.output(Field.course__name);
        q.output(Field.semester__id);

        Query q2 = new(Tbl.student);
        q2.Where(Field.student__id, stuId);
        q2.output(Field.student__name);

        string studentName = "";
        string courseName = "";

        void func(SqlConnection conn)
        {
            q.Select(conn, delegate (SqlDataReader reader)
            {
                int pos = 0;
                teacherName = DataReader.getStr(reader, ref pos);
                var fee = DataReader.getInt(reader, ref pos);
                this.fee = IoUtils.conv_fee(fee);
                courseName = DataReader.getStr(reader, ref pos);
                this.semesterId = DataReader.getInt(reader, ref pos);
            });

            q2.Select(conn, delegate (SqlDataReader reader)
            {
                studentName = DataReader.getStr(reader);
            });
        }

        QDatabase.exec(func);
        this.description = $"{stuId} - {studentName} - {courseName}";
    }

}