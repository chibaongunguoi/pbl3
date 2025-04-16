using Microsoft.Data.SqlClient;

static class CourseQuery
{
    public static void get_avg_rating(
        SqlConnection conn,
        int course_id,
        out double avg_rating,
        out int num_ratings
    )
    {
        Query q = new(Tbl.rating);
        q.Where(Field.rating__course_id, course_id);
        List<Rating> ratings = q.select<Rating>(conn);
        num_ratings = ratings.Count;

        int total_stars = 0;
        foreach (Rating rating in ratings)
        {
            total_stars += rating.stars;
        }
        avg_rating = num_ratings == 0 ? 0 : (double)total_stars / num_ratings;
    }
}

/* EOF */
