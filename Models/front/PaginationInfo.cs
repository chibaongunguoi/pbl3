public class PaginationInfo
{
    public int TotalItems { get; set; } = 0;
    public int ItemsPerPage { get; set; } = 1;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
}