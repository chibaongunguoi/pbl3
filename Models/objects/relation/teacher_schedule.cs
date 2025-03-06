using Microsoft.Data.SqlClient;

sealed class TeacherSchedule : DataObj
{
    // ========================================================================
    public int tch_id;
    public InfoInterval interval = new();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        tch_id = DataReader.get_int(reader, pos++);
        pos = interval.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString() => $"({tch_id},{interval.ToString()})";

    // ========================================================================
}

/* EOF */
