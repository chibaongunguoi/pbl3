using Microsoft.Data.SqlClient;

class DetailedCoursePage
{
    public int courseId;
    public List<BriefTeacherCard> teacherLst = new();
    public List<DetailedCourseCard> courseLst = new();

    public int maxPageNum;
    public double averageRating;
    public int numRatings;
    public Dictionary<int, int> rating_counts = new();

    public bool invalid => teacherLst.Count == 0 || courseLst.Count == 0;

    public DetailedCoursePage(int course_id, int num_objs = 10)
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

            this.courseLst = get_course_by_id(conn, course_id);
            this.teacherLst = get_teacher_by_id(conn, tch_id);

            q = new(Tbl.rating);
            q.Join(Field.semester__id, Field.rating__semester_id);
            q.Where(Field.semester__course_id, course_id);
            int count = q.Count(conn);
            this.maxPageNum = (int)Math.Ceiling((double)count / num_objs);

            Query rating_counts_q = new();
            for (int i = 1; i <= 5; i++)
            {
                Query q0 = new(Tbl.rating);
                q0.Join(Field.semester__id, Field.rating__semester_id);
                q0.Where(Field.semester__course_id, course_id);
                q0.Where(Field.rating__stars, i);
                q0.output(QPiece.countAll);
                rating_counts_q.outputQuery(q0.SelectQuery());
            }
            rating_counts_q.Select(
                conn,
                delegate(SqlDataReader reader)
                {
                    int pos = 0;
                    for (int i = 1; i <= 5; i++)
                    {
                        int count = DataReader.getInt(reader, pos++);
                        rating_counts.Add(i, count);
                    }
                }
            );
        }
        // thay thees delegate(SqlConnection conn);
        QDatabase.exec(func);
    }

    public static List<RatingCard> get_page(
        SqlConnection conn,
        int courseId,
        int page = 1,
        int num_objs = 10
    )
    {
        List<RatingCard> cards = new();
        Query q = RatingCard.get_query_creator();
        q.Where(Field.semester__course_id, courseId);
        q.OrderBy(Field.rating__timestamp, desc: true);
        q.offset(page, num_objs);
        q.Select(conn, reader => cards.Add(RatingCard.get_card(conn, reader)));
        return cards;
    }

    public static List<BriefTeacherCard> get_teacher_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Tbl.teacher);
        q.Where(Field.teacher__id, tch_id);
        q.Select(conn, reader => cards.Add(DataReader.getDataObj<BriefTeacherCard>(reader)));
        return cards;
    }

    public static List<DetailedCourseCard> get_course_by_id(SqlConnection conn, int id)
    {
        List<DetailedCourseCard> cards = new();
        Query q = DetailedCourseCard.getQueryCreator();
        q.Where(Field.course__id, id);
        q.Select(conn, reader => cards.Add(DataReader.getDataObj<DetailedCourseCard>(reader)));
        return cards;
    }
}
