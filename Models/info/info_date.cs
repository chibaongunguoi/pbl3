using Microsoft.Data.SqlClient;

sealed class InfoDate : DataObj
{
    // ========================================================================
    public int year;
    public int month;
    public int day;

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        DateTime date = reader.GetDateTime(pos++);
        year = date.Year;
        month = date.Month;
        day = date.Day;
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return $"{year}-{month:D2}-{day:D2}";
    }

    // ========================================================================
}

/* EOF */
