using Microsoft.Data.SqlClient;

sealed class ContractSchedule : DataObj
{
    // ========================================================================
    public int ctrct_id;
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        ctrct_id = DataReader.get_int(reader, pos++);
        pos = interval.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString() => $"({ctrct_id},{interval.ToString()})";

    // ========================================================================
}

/* EOF */
