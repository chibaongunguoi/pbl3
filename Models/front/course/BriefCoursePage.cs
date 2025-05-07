// Trang tổng quan các khóa học
class BriefCoursePage
{
    public PaginationInfo MPaginationInfo { get; set; } = new() {ItemsPerPage = 20};
    public BriefCourseFilter MBriefCourseFilter { get; set; } = new();
    public BriefCoursePage(BriefCourseFilter filter)
    {
        MBriefCourseFilter = filter;
        QDatabase.Exec(
            conn =>
            {
                MBriefCourseFilter.Reset(conn);
                MPaginationInfo.CurrentPage = 1;
            }
        );
    }
}

/* EOF */
