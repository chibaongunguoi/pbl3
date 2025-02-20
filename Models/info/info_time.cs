namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoTime : DataObj
{
    // ========================================================================
    public int hour;
    public int minute;

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
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
