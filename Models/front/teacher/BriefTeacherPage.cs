using Microsoft.Data.SqlClient;

class BriefTeacherPage
{
    const int NUM_DISPLAYED_OBJS = 20;
    public PaginationInfo MPaginationInfo { get; set; } = new();
    public BriefTeacherPage()
    {
        QDatabase.Exec(
            conn =>
            {
                Query q = new(Tbl.teacher);
                int total_num = q.Count(conn);
                MPaginationInfo.TotalItems = total_num;
                MPaginationInfo.ItemsPerPage = NUM_DISPLAYED_OBJS;
                MPaginationInfo.CurrentPage = 1;
            }
        );
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> GetPage(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20
    )
    {
        List<BriefTeacherCard> cards = [];
        Query q = new(Tbl.teacher);
        q.Offset(page, num_objs);
        q.Select(conn, reader => cards.Add(QDataReader.GetDataObj<BriefTeacherCard>(reader)));
        return cards;
    }
}

/* EOF */
