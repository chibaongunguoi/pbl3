public class ScheduleUtils
{
    // ========================================================================
    // INFO: Trả về danh sách các phần tử trong A mà không có trong B.
    public List<int> subtract(List<int> A, List<int> B)
    {
        List<int> result = new List<int>();
        foreach (int a in A)
        {
            if (!B.Contains(a))
            {
                result.Add(a);
            }
        }
        return result;
    }

    // ========================================================================
    public static string conv(string status)
    {
        switch(status)
        {
            case CourseStatus.waiting:
                return "Chưa bắt đầu";
            case CourseStatus.started:
                return "Đang dạy";
            case CourseStatus.finished:
                return "Đã hoàn thành";
            default:
                return "";
        }
    }

    // ========================================================================
}

/* EOF */
