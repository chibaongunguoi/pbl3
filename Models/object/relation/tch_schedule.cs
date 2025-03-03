using Microsoft.Data.SqlClient;

sealed class TchSchedule : DataObj
{
    // ========================================================================
    public int tch_id;
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.tch_id = reader.GetInt32(pos++);
        pos = this.interval.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return $"({this.tch_id}, {this.interval.ToString()})";
    }

    // ========================================================================
}

/* EOF */
