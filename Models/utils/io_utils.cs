class IoUtils
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
    public static string conv(InfoGender gender)
    {
        switch (gender)
        {
            case InfoGender.MALE:
                return "Nam";
            case InfoGender.FEMALE:
                return "Nữ";
            default:
                return "";
        }
    }

    // ------------------------------------------------------------------------
    public static string conv(InfoDate date)
    {
        return $"{date.day:D2}/{date.month:D2}/{date.year}";
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
