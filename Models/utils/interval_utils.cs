class IntervalUtils
{
    // ========================================================================
    public static List<InfoInterval> normalize(List<InfoInterval> intervals)
    {
        intervals.Sort(
            (a, b) =>
                a < b ? -1
                : a > b ? 1
                : 0
        );
        InfoInterval? current = null;
        List<InfoInterval> result = new();

        for (int i = 0; i < intervals.Count; i++)
        {
            if (current is null)
            {
                current = intervals[i];
            }
            else if (current.intersects(intervals[i]))
            {
                current = current.union(intervals[i]);
            }
            else
            {
                result.Add(current);
                current = intervals[i];
            }
        }

        if (current is not null)
        {
            result.Add(current);
        }

        return result;
    }

    // ========================================================================
    public static List<InfoInterval> get_intersection(
        List<InfoInterval> intervals_1,
        List<InfoInterval> intervals_2
    )
    {
        List<InfoInterval> result = new();
        foreach (var interval1 in intervals_1)
        {
            foreach (var interval2 in intervals_2)
            {
                if (interval1.intersects(interval2))
                {
                    result.Add(interval1.intersection(interval2));
                }
            }
        }
        return result;
    }

    // ------------------------------------------------------------------------
    public static List<InfoInterval> get_subtraction(
        List<InfoInterval> intervals_1,
        List<InfoInterval> intervals_2
    )
    {
        List<InfoInterval> result = new(intervals_1);

        foreach (var interval2 in intervals_2)
        {
            for (int i = 0; i < result.Count; i++)
            {
                var interval1 = result[i];
                if (interval1.intersects(interval2))
                {
                    result.RemoveAt(i);
                    if (interval1.start < interval2.start)
                    {
                        result.Insert(
                            i,
                            new InfoInterval
                            {
                                day = interval1.day,
                                start = interval1.start,
                                end = interval2.start,
                            }
                        );
                        i++;
                    }
                    if (interval1.end > interval2.end)
                    {
                        result.Insert(
                            i,
                            new InfoInterval
                            {
                                day = interval1.day,
                                start = interval2.end,
                                end = interval1.end,
                            }
                        );
                    }
                }
            }
        }

        return result;
    }

    // ========================================================================
    public static bool suitable(List<InfoInterval> intervals, int slots, int duration)
    {
        return true;
    }

    // ========================================================================
}

/* EOF */
