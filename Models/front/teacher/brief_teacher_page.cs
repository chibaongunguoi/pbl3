using Microsoft.Data.SqlClient;

class BriefTeacherPage
{
    public int total_num_pages;
    public List<BriefTeacherCard> teachers = new();

    public BriefTeacherPage(int current_page = 1, int num_displayed_objs = 20)
    {
        Database.exec(
            delegate(SqlConnection conn)
            {
                Query q = new(Tbl.teacher);
                int total_num = q.Count(conn);
                this.total_num_pages = (int)Math.Ceiling((double)total_num / num_displayed_objs);
                this.teachers = BriefTeacherPage.get_page(conn, current_page, num_displayed_objs);
            }
        );
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_page(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20
    )
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Tbl.teacher);
        q.offset(page, num_objs);
        return q.Select<BriefTeacherCard>(conn);
    }
}

/* EOF */
