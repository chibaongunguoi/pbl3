using Microsoft.Data.SqlClient;

public class ManageSemesterPage
{
    public int CourseId;
    public ManageSemesterPage(int courseId)
    {
        CourseId = courseId;
        // QDatabase.Exec(
        //     delegate(SqlConnection conn)
        //     {
        //         int tableIdx = 1;
        //         Query q = SemesterCard.get_query_creator();
        //         q.Where(Field.semester__course_id, courseId);
        //         q.OrderBy(Field.semester__start_date, desc: true);
        //         q.Select(
        //             conn,
        //             reader => Semesters.Add(SemesterCard.get_card(reader, ref tableIdx))
        //         );
        //     }
        // );
    }
}

/* EOF */
