using Microsoft.Data.SqlClient;

sealed class InfoTime : DataObj
{
    // ========================================================================
    public int hour;
    public int minute;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data(reader, pos);
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
}

/* EOF */
