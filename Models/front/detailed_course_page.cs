using Microsoft.Data.SqlClient;

class DetailedCoursePage
{
    public List<BriefTeacherCard> teachers = new();
    public List<DetailedCourseCard> courses = new();

    public bool invalid => teachers.Count == 0 || courses.Count == 0;

    public DetailedCoursePage(int course_id)
    {
        void func(SqlConnection conn)
        {
            Query q = new(Table.course);
            q.where_(Field.course__id, course_id);
            List<Course> courses = q.select<Course>(conn);
            if (courses.Count == 0)
                return;

            int tch_id = courses[0].tch_id;

            this.courses = DetailedCourseCard.get_by_id(conn, course_id);
            this.teachers = BriefTeacherCard.get_by_id(conn, tch_id);
        }

        Database.exec(func);
    }
}
