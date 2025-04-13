using System.ComponentModel;
using Microsoft.Data.SqlClient;

class DetailedCoursePage
{
    public List<BriefTeacherCard> teachers = new();
    public List<DetailedCourseCard> courses = new();

    public List<RatingCard> cards = new();

    public double averageRating;
    
    public int numRatings;

    Dictionary<int, int> ratingCount = new();

    public bool invalid => teachers.Count == 0 || courses.Count == 0;

    public DetailedCoursePage(int course_id, int current_page = 1, int num_objs = 10) 
    {
        void func(SqlConnection conn)
        {
            CourseQuery.get_avg_rating(conn, course_id, out averageRating, out numRatings);    
            Query q = new(Table.course);
            q.where_(Field.course__id, course_id);
            List<Course> courses = q.select<Course>(conn);
            if (courses.Count == 0)
                return;

            int tch_id = courses[0].tch_id;

            this.courses = get_course_by_id(conn, course_id);
            this.teachers = get_teacher_by_id(conn, tch_id);
            this.cards = get_page(
                conn, course_id, current_page, num_objs
            );
        }
        // thay thees delegate(SqlConnection conn);
        Database.exec(func);
    }

    public static List<RatingCard> get_page(SqlConnection conn, int courseId, 
    int page = 1,
    int num_objs = 10
    ) 
    {
        List<RatingCard>  cards = new();
        Query q = RatingCard.get_query_creator();
        q.offset(page, num_objs);
        q.where_(Field.course__id, courseId);

        q.select(conn, reader => cards.Add(RatingCard.get_card(conn, reader)));
        return cards;
    }


    public static List<BriefTeacherCard> get_teacher_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, tch_id);
        q.select(conn, reader => cards.Add(BriefTeacherCard.get_card(conn, reader)));
        return cards;
    }

    public static List<DetailedCourseCard> get_course_by_id(SqlConnection conn, int id)
    {
        List<DetailedCourseCard> cards = new();
        Query q = DetailedCourseCard.get_query_creator();
        q.where_(Field.course__id, id);
        q.select(conn, reader => DetailedCourseCard.get_card(conn, reader, ref cards));
        return cards;
    }

}
