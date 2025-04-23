using Microsoft.Data.SqlClient;

class BriefTeacherPage
{
    public int maxPageNum;
    public BriefTeacherPage(int num_displayed_objs = 20)
    {
        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                Query q = new(Tbl.teacher);
                int total_num = q.Count(conn);
                this.maxPageNum = (int)Math.Ceiling((double)total_num / num_displayed_objs);
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
        q.Offset(page, num_objs);
        q.Select(conn, reader => cards.Add(DataReader.getDataObj<BriefTeacherCard>(reader)));
        return cards;
    }
}

/* EOF */
