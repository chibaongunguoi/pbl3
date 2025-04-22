using Microsoft.Data.SqlClient;

class DetailedTeacherPage
{
    public int tchId;
    public int maxPageNum = 0;
    public List<DetailedTeacherCard> teacherCard = new();
    public List<BriefCourseCard> courses = new();

    public bool success => teacherCard.Count > 0;

    public DetailedTeacherPage(int tchId, int numObjs = 20)
    {
        this.tchId = tchId;
        QDatabase.exec(
            delegate(SqlConnection conn)
            {
                Query q = new(Tbl.course);
                q.Where(Field.course__tch_id, tchId);
                q.output(QPiece.countAll);
                q.select(conn, reader => maxPageNum = DataReader.getInt(reader));
                Query q1 = DetailedTeacherCard.getQueryCreator();
                q1.Where(Field.teacher__id, tchId);
                q1.select(conn, reader => teacherCard.Add(DetailedTeacherCard.getCard(reader)));
                this.courses = getCourses(conn, tchId, 1, numObjs);
            }
        );
    }

    public static List<BriefCourseCard> getCourses(
        SqlConnection conn,
        int tchId,
        int currentPage,
        int numObjs = 20
    )
    {
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.offset(currentPage, numObjs);
        q.select(conn, reader => cards.Add(DataReader.getDataObj<BriefCourseCard>(reader)));
        return cards;
    }
}
