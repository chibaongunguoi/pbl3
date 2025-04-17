static class Checking
{
    public static void check_start_finish_dates(
        ref Dictionary<string, string> errors,
        DateOnly? start_date,
        DateOnly? finish_date
    )
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (start_date is null)
        {
            errors[ErrorKey.start_date_missing] = "Ngày bắt đầu không được để trống";
        }
        else if (start_date < today)
        {
            errors[ErrorKey.start_date_invalid] = "Ngày bắt đầu không hợp lệ";
        }
        else if (finish_date is null)
        {
            errors[ErrorKey.finish_date_missing] = "Ngày kết thúc không được để trống";
        }
        else if (finish_date < start_date)
        {
            errors[ErrorKey.finish_date_invalid] = "Ngày kết thúc không hợp lệ";
        }
    }
}
