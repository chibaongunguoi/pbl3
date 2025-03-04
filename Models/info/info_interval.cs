using Microsoft.Data.SqlClient;

sealed class InfoInterval : DataObj
{
    // ========================================================================
    public InfoDay day = new();
    public InfoTime start_time = new();
    public InfoTime end_time = new();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data(reader, pos);
        pos = this.start_time.fetch_data(reader, pos);
        pos = this.end_time.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return $"{start_time.ToString()} - {end_time.ToString()}";
    }

    // ========================================================================
}

/* EOF */
