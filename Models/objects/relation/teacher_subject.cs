using Microsoft.Data.SqlClient;

class TeacherSubject : DataObj
{
    // ========================================================================
    public int tch_id;
    public int sbj_id;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        tch_id = DataReader.get_int(reader, pos++);
        sbj_id = DataReader.get_int(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString() => $"{tch_id},{sbj_id}";

    // ========================================================================
}

/* EOF */
