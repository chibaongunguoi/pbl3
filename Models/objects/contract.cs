using Microsoft.Data.SqlClient;

sealed class Contract : IdObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int tch_id { get; set; }
    public int sbj_id { get; set; }
    public InfoDate start_date { get; set; } = new InfoDate();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        stu_id = DataReader.get_int(reader, pos++);
        tch_id = DataReader.get_int(reader, pos++);
        sbj_id = DataReader.get_int(reader, pos++);
        pos = start_date.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
