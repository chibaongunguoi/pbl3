using Microsoft.Data.SqlClient;

class DetailedTeacherPage
{
    public PaginationInfo MPaginationInfo = new() { ItemsPerPage = 20 };
    public DetailedTeacherCard? teacherCard = null;
    public BriefCourseFilter MBriefCourseFilter = new();

    public DetailedTeacherPage(int tchId, BriefCourseFilter filter)
    {
        QDatabase.Exec(
            conn =>
            {
                Query q = new(Tbl.course);
                q.Where(Field.course__tch_id, tchId);
                q.Output(QPiece.countAll);
                q.Select(conn, reader => MPaginationInfo.TotalItems = QDataReader.GetInt(reader));

                Query q1 = DetailedTeacherCard.getQueryCreator();
                q1.Where(Field.teacher__id, tchId);
                q1.Select(conn, reader => teacherCard = DetailedTeacherCard.getCard(reader));

                filter.Reset(conn);
            }
        );

        MBriefCourseFilter = filter;
    }

    public static List<BriefCourseCard> GetCourses(
        SqlConnection conn,
        int tchId,
        int currentPage,
        int numObjs = 20
    )
    {
        List<BriefCourseCard> cards = [];
        Query q = BriefCourseCard.GetQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.Offset(currentPage, numObjs);
        q.Select(conn, reader => cards.Add(QDataReader.GetDataObj<BriefCourseCard>(reader)));
        return cards;
    }
}
