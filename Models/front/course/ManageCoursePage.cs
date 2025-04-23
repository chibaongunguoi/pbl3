using Microsoft.Data.SqlClient;

class ManageCoursePage
{
    public List<ManageCourseCard> cards = new();

    public static ManageCoursePage get_by_tch_id(int tch_id, int page_idx = 1, int num_objs = 20)
    {
        ManageCoursePage page = new();
        Query q = ManageCourseCard.GetQueryCreator();
        q.Where(Field.course__tch_id, tch_id);
        q.Offset(page_idx, num_objs);

        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                int table_index = 1;
                int pos = 0;
                q.Select(
                    conn,
                    reader => page.cards.Add(ManageCourseCard.GetCard(reader, ref pos, ref table_index))
                );
            }
        );
        return page;
    }
}
