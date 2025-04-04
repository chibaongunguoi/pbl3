using Microsoft.Data.SqlClient;

struct BriefCoursePage
{
    public int current_page;
    public int total_num_pages;
    public List<BriefCourseCard> courses = new();

    public BriefCoursePage(int current_page = 1, int num_displayed_courses = 20)
    {
        var self = this;
        void func(SqlConnection conn)
        {
            Query q = new(Table.course);
            int num_total_courses = q.count(conn);

            self.current_page = current_page;
            self.total_num_pages = (int)
                Math.Ceiling((double)num_total_courses / num_displayed_courses);
            self.courses = BriefCourseCard.get_page(conn, current_page, num_displayed_courses);
        }

        Database.exec(func);
        this = self;
    }
}

/* EOF */
