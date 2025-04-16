using Microsoft.Data.SqlClient;

class DetailedCoursePage
{
    public int course_id;
    public int current_page;
    public List<BriefTeacherCard> teacher_lst = new();
    public List<DetailedCourseCard> course_lst = new();

    public List<RatingCard> cards = new();

    public int total_num_pages;
    public double averageRating;
    public int numRatings;
    public Dictionary<int, int> rating_counts = new();

    public bool invalid => teacher_lst.Count == 0 || course_lst.Count == 0;

    public DetailedCoursePage(int course_id, int current_page = 1, int num_objs = 10)
    {
        this.course_id = course_id;
        this.current_page = current_page;
        void func(SqlConnection conn)
        {
            CourseQuery.get_avg_rating(conn, course_id, out averageRating, out numRatings);
            Query q = new(Tbl.course);
            q.Where(Field.course__id, course_id);
            List<Course> course_lst = q.select<Course>(conn);
            if (course_lst.Count == 0)
                return;

            int tch_id = course_lst[0].tch_id;

            this.course_lst = get_course_by_id(conn, course_id);
            this.teacher_lst = get_teacher_by_id(conn, tch_id);
            this.cards = get_page(conn, course_id, current_page, num_objs);

            q = RatingCard.get_query_creator();
            q.Where(Field.rating__course_id, course_id);
            int count = q.count(conn);
            this.total_num_pages = (int)Math.Ceiling((double)count / num_objs);

            for (int i = 1; i <= 5; i++)
            {
                q = RatingCard.get_query_creator();
                q.Where(Field.rating__stars, i);
                q.Where(Field.rating__course_id, course_id);
                rating_counts.Add(i, q.count(conn));
            }
        }
        // thay thees delegate(SqlConnection conn);
        Database.exec(func);
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
        q.Where(Field.rating__course_id, courseId);
        q.offset(page, num_objs);
        q.select(conn, reader => cards.Add(RatingCard.get_card(conn, reader)));
        return cards;
    }

    public static List<BriefTeacherCard> get_teacher_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Tbl.teacher);
        q.Where(Field.teacher__id, tch_id);
        return q.select<BriefTeacherCard>(conn);
    }

    public static List<DetailedCourseCard> get_course_by_id(SqlConnection conn, int id)
    {
        List<DetailedCourseCard> cards = new();
        Query q = DetailedCourseCard.get_query_creator();
        q.Where(Field.course__id, id);
        return q.select<DetailedCourseCard>(conn);
    }
}
