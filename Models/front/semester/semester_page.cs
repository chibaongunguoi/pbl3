using Microsoft.Data.SqlClient;
class SemesterPage
{
    public List<SemesterCard> semesters { get; set; } = new();
    public static int num_pages { get; set; } = 0;

    public static SemesterPage getPage(int teacher_id, int page_ix, int num_objs = 15) {
        SemesterPage page = new();
        Query q = SemesterCard.get_query_creator();
    	q.Where(Field.course__tch_id, teacher_id);
        q.offset(page_ix, num_objs);

        string connectionString = ConfigOptionManager.get_server_name(); 
        SqlConnection conn = new SqlConnection(connectionString);

        num_pages = q.count(conn);

        Database.exec(
            delegate (SqlConnection conn)
            {
                int table_index = 1;
                q.select(
                    conn,
                    reader => page.semesters.Add(SemesterCard.get_card(reader, ref table_index))
                );
            }
        );
        return page;
    }
}