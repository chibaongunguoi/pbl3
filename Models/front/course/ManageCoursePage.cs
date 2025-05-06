using Microsoft.Data.SqlClient;

class ManageCoursePage
{
    public List<ManageCourseCard> cards = new();

    public static ManageCoursePage GetByTeacherUsername(string username, int page_idx = 1, int num_objs = 20)
    {
        Query tchIdQ = new(Tbl.teacher);
        tchIdQ.Where(Field.teacher__username, username);
        tchIdQ.Output(Field.teacher__id);

        ManageCoursePage page = new();
        Query q = ManageCourseCard.GetQueryCreator();
        q.WhereQuery(Field.course__tch_id, tchIdQ.SelectQuery());
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
