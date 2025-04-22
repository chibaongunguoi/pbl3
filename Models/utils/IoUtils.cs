public class IoUtils
{
    // ========================================================================
    // INFO: In lần lượt các phần tử của một list ra màn hình.
    public static void print_list<T>(List<T> l)
    {
        Console.WriteLine("Output:");
        foreach (var e in l)
        {
            if (e != null)
                Console.WriteLine(e.ToString());
        }
        Console.WriteLine("End of Output.");
    }

    // ========================================================================
    public static string convGender(string gender)
    {
        switch (gender)
        {
            case Gender.male:
                return "Nam";
            case Gender.female:
                return "Nữ";
            default:
                return "";
        }
    }

    // ------------------------------------------------------------------------
    public static string conv(DateOnly date)
    {
        return $"{date.Day:D2}/{date.Month:D2}/{date.Year}";
    }

    // ------------------------------------------------------------------------
    public static string conv(DateTime date)
    {
        return $"{date.Day:D2}/{date.Month:D2}/{date.Year} {date.Hour:D2}:{date.Minute:D2}";
    }

    // ------------------------------------------------------------------------
    public static string conv_db(DateOnly date)
    {
        return $"";
    }

    // ------------------------------------------------------------------------
    // 300000000 -> 300.000.000
    public static string conv_fee(int fee)
    {
        return fee.ToString("N0", System.Globalization.CultureInfo.InvariantCulture)
            .Replace(",", ".");
    }

    // ========================================================================
}

/* EOF */
