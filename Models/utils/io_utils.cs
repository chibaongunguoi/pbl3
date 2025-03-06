class IoUtils
{
    // ========================================================================
    public static void print_list<T>(List<T> l)
    {
        foreach (var e in l)
        {
            if (e != null)
                Console.WriteLine(e.ToString());
        }
    }

    // ========================================================================
}

/* EOF */
