using Microsoft.Data.SqlClient;

class DetailedCoursePage
{
    public int courseId;
    public BriefTeacherCard? teacher = null;
    public DetailedCourseCard? course = null;
    public double averageRating;
    public int numRatings;
    public Dictionary<int, int> rating_counts = new();
    public PaginationInfo MPaginationInfo = new() { ItemsPerPage = 10 };
    public DetailedCoursePage(int course_id)
    {
        this.courseId = course_id;
        void func(SqlConnection conn)
        {
            CourseQuery.get_avg_rating(conn, course_id, out averageRating, out numRatings);
            Query q = new(Tbl.course);
            q.Where(Field.course__id, course_id);
            List<Course> course_lst = q.Select<Course>(conn);
            if (course_lst.Count == 0)
                return;

            int tch_id = course_lst[0].TchId;

            course = get_course_by_id(conn, course_id);
            teacher = get_teacher_by_id(conn, tch_id);

            q = new(Tbl.rating);
            q.Join(Field.semester__id, Field.rating__semester_id);
            q.Where(Field.semester__course_id, course_id);
            int count = q.Count(conn);

            Query rating_counts_q = new();
            for (int i = 1; i <= 5; i++)
            {
                Query q0 = new(Tbl.rating);
                q0.Join(Field.semester__id, Field.rating__semester_id);
                q0.Where(Field.semester__course_id, course_id);
                q0.Where(Field.rating__stars, i);
                q0.Output(QPiece.countAll);
                rating_counts_q.OutputQuery(q0.SelectQuery());
            }
            rating_counts_q.Select(
                conn,
                delegate(SqlDataReader reader)
                {
                    int pos = 0;
                    for (int i = 1; i <= 5; i++)
                    {
                        int count = QDataReader.GetInt(reader, pos++);
                        rating_counts.Add(i, count);
                    }
                }
            );
        }
        QDatabase.Exec(func);
    }

    public static BriefTeacherCard? get_teacher_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Tbl.teacher);
        q.Where(Field.teacher__id, tch_id);
        q.Select(conn, reader => cards.Add(QDataReader.GetDataObj<BriefTeacherCard>(reader)));
        return cards.Count > 0 ? cards[0] : null;
    }

    public static DetailedCourseCard? get_course_by_id(SqlConnection conn, int id)
    {
        List<DetailedCourseCard> cards = [];
        Query q = DetailedCourseCard.GetQueryCreator();
        q.Where(Field.course__id, id);
        q.Select(conn, reader => cards.Add(QDataReader.GetDataObj<DetailedCourseCard>(reader)));
        return cards.Count > 0 ? cards[0] : null;
    }
}
