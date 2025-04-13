using Microsoft.Data.SqlClient;

class BriefTeacherPage
{
    public int current_page;
    public int total_num_pages;
    public List<BriefTeacherCard> teachers = new();

    public BriefTeacherPage(int current_page = 1, int num_displayed_objs = 20)
    {
        Database.exec(
            delegate(SqlConnection conn)
            {
                Query q = new(Table.teacher);
                int total_num = q.count(conn);

                this.current_page = current_page;
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
        Query q = new(Table.teacher);
        q.offset(page, num_objs);
        q.select(conn, reader => cards.Add(BriefTeacherCard.get_card(conn, reader)));
        return cards;
    }
}

/* EOF */
