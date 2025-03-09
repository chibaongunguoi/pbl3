class ScheduleUtils
{
    // ========================================================================
    // INFO: Trả về danh sách các phần tử trong A mà không có trong B.
    List<int> subtract(List<int> A, List<int> B)
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
}

/* EOF */
