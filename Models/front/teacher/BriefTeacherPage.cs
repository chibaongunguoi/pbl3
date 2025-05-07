public class BriefTeacherPage
{
    public PaginationInfo MPaginationInfo { get; set; } = new() { ItemsPerPage = 20 };
    public BriefTeacherFilter MBriefTeacherFilter { get; set; } = new();
    public BriefTeacherPage(BriefTeacherFilter filter)
    {
        MPaginationInfo.CurrentPage = 1;
        MBriefTeacherFilter = filter;
    }
}

/* EOF */
