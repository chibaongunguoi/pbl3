using Microsoft.Data.SqlClient;

sealed class TeacherSchedule : DataObj
{
    // ========================================================================
    public int tch_id;
    public int sch_id;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        tch_id = DataReader.get_int(reader, pos++);
        sch_id = DataReader.get_int(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString() => $"{tch_id},{sch_id}";

    // ========================================================================
}

/* EOF */
