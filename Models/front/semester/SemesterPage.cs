using Microsoft.Data.SqlClient;

class SemesterPage
{
    public int courseId;
    public List<SemesterCard> semesters { get; set; } = new();

    public SemesterPage(int courseId)
    {
        this.courseId = courseId;
        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                int tableIdx = 1;
                Query q = SemesterCard.get_query_creator();
                q.Where(Field.semester__course_id, courseId);
                q.OrderBy(Field.semester__start_date, desc: true);
                q.Select(
                    conn,
                    reader => semesters.Add(SemesterCard.get_card(reader, ref tableIdx))
                );
            }
        );
    }
}

/* EOF */
