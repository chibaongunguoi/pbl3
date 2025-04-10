using Microsoft.Data.SqlClient;

class ManageCoursePage
{
    public List<ManageCourseCard> cards = new();

    public static ManageCoursePage get_by_tch_id(int tch_id, int page_idx = 1, int num_objs = 20)
    {
        ManageCoursePage page = new();
        Query q = ManageCourseCard.get_query_creator();
        q.where_(Field.course__tch_id, tch_id);
        q.offset(page_idx, num_objs);

        Database.exec(
            delegate(SqlConnection conn)
            {
                int table_index = 1;
                q.select(
                    conn,
                    reader =>
                        page.cards.Add(ManageCourseCard.get_card(conn, reader, ref table_index))
                );
            }
        );
        return page;
    }
}
