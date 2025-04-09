using Microsoft.Data.SqlClient;

sealed class InfoTime : DataObj
{
    // ========================================================================
    public int hour;
    public int minute;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        TimeSpan time = reader.GetTimeSpan(pos++);
        this.hour = time.Hours;
        this.minute = time.Minutes;
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return $"{hour:D2}:{minute:D2}";
    }

    // ========================================================================
    public static bool operator ==(InfoTime a, InfoTime b)
    {
        return a.hour == b.hour && a.minute == b.minute;
    }

    // ------------------------------------------------------------------------
    public static bool operator !=(InfoTime a, InfoTime b) => !(a == b);

    // ------------------------------------------------------------------------
    public override bool Equals(object? obj)
    {
        if (obj is InfoTime)
        {
            return this == (InfoTime)obj;
        }
        return false;
    }

    // ------------------------------------------------------------------------
    public override int GetHashCode() => base.GetHashCode();

    // ========================================================================
    public static bool operator <(InfoTime a, InfoTime b)
    {
        if (a.hour < b.hour)
        {
            return true;
        }
        else if (a.hour == b.hour)
        {
            return a.minute < b.minute;
        }
        return false;
    }

    // ------------------------------------------------------------------------
    public static bool operator >(InfoTime a, InfoTime b) => !(a < b) && a != b;

    // ------------------------------------------------------------------------
    public static bool operator <=(InfoTime a, InfoTime b) => a < b || a == b;

    // ------------------------------------------------------------------------
    public static bool operator >=(InfoTime a, InfoTime b) => a > b || a == b;
    // ========================================================================
}

/* EOF */
