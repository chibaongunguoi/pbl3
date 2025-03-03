namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;
using module_info;

sealed class CtrctSchedule : DataObj
{
    // ========================================================================
    public int ctrct_id;
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.ctrct_id = reader.GetInt32(pos++);
        pos = this.interval.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return $"({this.ctrct_id}, {this.interval.ToString()})";
    }

    // ========================================================================
}

/* EOF */
