using Microsoft.Data.SqlClient;

sealed class InfoInterval : DataObj
{
    // ========================================================================
    public InfoDay day = new();
    public InfoTime start = new();
    public InfoTime end = new();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data(reader, pos);
        day = DataReader.get_enum<InfoDay>(reader, pos++);
        pos = this.start.fetch_data(reader, pos);
        pos = this.end.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return $"{((int)day).ToString()},{start.ToString()},{end.ToString()}";
    }

    // ========================================================================
    public static bool operator ==(InfoInterval a, InfoInterval b)
    {
        return a.day == b.day && a.start == b.start && a.end == b.end;
    }

    // ------------------------------------------------------------------------
    public static bool operator !=(InfoInterval a, InfoInterval b) => !(a == b);

    // ------------------------------------------------------------------------
    public override bool Equals(object? obj)
    {
        if (obj is InfoInterval)
        {
            return this == (InfoInterval)obj;
        }
        return false;
    }

    // ------------------------------------------------------------------------
    public override int GetHashCode() => base.GetHashCode();

    // ========================================================================
    public static bool operator <(InfoInterval a, InfoInterval b)
    {
        if (a.day < b.day)
        {
            return true;
        }
        else if (a.day == b.day)
        {
            if (a.start < b.start)
            {
                return true;
            }
            else if (a.start == b.start)
            {
                return a.end < b.end;
            }
        }
        return false;
    }

    // ------------------------------------------------------------------------
    public static bool operator >(InfoInterval a, InfoInterval b) => !(a < b) && a != b;

    // ------------------------------------------------------------------------
    public static bool operator <=(InfoInterval a, InfoInterval b) => a < b || a == b;

    // ------------------------------------------------------------------------
    public static bool operator >=(InfoInterval a, InfoInterval b) => a > b || a == b;

    // ========================================================================
    public bool intersects(InfoInterval other)
    {
        return this.day == other.day && (this.start < other.end && this.end > other.start);
    }

    // ------------------------------------------------------------------------
    public InfoInterval intersection(InfoInterval other)
    {
        InfoInterval result = new();
        result.day = this.day;
        result.start = this.start < other.start ? other.start : this.start;
        result.end = this.end < other.end ? this.end : other.end;
        return result;
    }

    // ------------------------------------------------------------------------
    public InfoInterval union(InfoInterval other)
    {
        return new()
        {
            day = this.day,
            start = this.start < other.start ? this.start : other.start,
            end = this.end > other.end ? this.end : other.end,
        };
    }
    // ========================================================================
}

/* EOF */
