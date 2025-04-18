using Microsoft.Data.SqlClient;

class SemesterPage
{
    public int courseId;
    public List<SemesterCard> semesters { get; set; } = new();

    public SemesterPage(int courseId)
    {
        this.courseId = courseId;
        Database.exec(
            delegate(SqlConnection conn)
            {
                int tableIdx = 1;
                Query q = SemesterCard.get_query_creator();
                q.Where(Field.semester__course_id, courseId);
                q.orderBy(Field.semester__start_date, desc: true);
                q.select(
                    conn,
                    reader => semesters.Add(SemesterCard.get_card(reader, ref tableIdx))
                );
            }
        );
    }
}

/* EOF */
