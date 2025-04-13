class RatingPage{
    public List <RatingCard> RatingCards { get; set; } = new();
    public int CourseId { get; set; }
    public int current_page = 1;
    int num_displayed_course = 10;




}