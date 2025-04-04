using Microsoft.Data.SqlClient;

struct BriefTeacherPage
{
    public int current_page;
    public int total_num_pages;
    public List<BriefCourseCard> teachers = new();

    public BriefTeacherPage(int current_page = 1, int num_displayed_objs = 20)
    {
        var self = this;
        void func(SqlConnection conn)
        {
            Query q = new(Table.teacher);
            int total_num = q.count(conn);

            self.current_page = current_page;
            self.total_num_pages = (int)Math.Ceiling((double)total_num / num_displayed_objs);
            self.teachers = BriefCourseCard.get_page(conn, current_page, num_displayed_objs);
        }

        Database.exec(func);
        this = self;
    }
}

/* EOF */
