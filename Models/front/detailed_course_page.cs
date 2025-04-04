using Microsoft.Data.SqlClient;

struct DetailedCoursePage
{
    BriefTeacherCard? teacher_card;
    DetailedCourseCard? course_card;

    public DetailedCoursePage(int course_id)
    {
        var self = this;
        void func(SqlConnection conn)
        {
            Query q = new(Table.course);
            q.where_(Field.course__id, course_id);
            List<Course> courses = q.select<Course>(conn);
            if (courses.Count == 0)
                return;

            int tch_id = courses[0].tch_id;

            self.course_card = DetailedCourseCard.get_by_id(conn, course_id);
            self.teacher_card = BriefTeacherCard.get_by_id(conn, tch_id);
        }

        Database.exec(func);
        this = self;
    }
}
