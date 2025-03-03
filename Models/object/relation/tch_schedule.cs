using Microsoft.Data.SqlClient;

sealed class TchSchedule : DataObj
{
    // ========================================================================
    public int tch_id;
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data(reader, pos);
        tch_id = DataReader.get_int(reader, pos++);
        pos = interval.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return $"({tch_id}, {interval.ToString()})";
    }

    // ========================================================================
}

/* EOF */
