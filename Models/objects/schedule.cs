using Microsoft.Data.SqlClient;

class Schedule : IdObj
{
    // ========================================================================
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        pos = interval.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
