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
}

/* EOF */
