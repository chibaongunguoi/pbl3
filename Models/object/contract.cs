using Microsoft.Data.SqlClient;

class Contract : DataObj
{
    // ========================================================================
    public int id;
    public int stu_id;
    public int tch_id;
    public int sbj_id;
    public InfoDate start_date = new InfoDate();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data(reader, pos);
        id = DataReader.get_int(reader, pos++);
        stu_id = DataReader.get_int(reader, pos++);
        tch_id = DataReader.get_int(reader, pos++);
        sbj_id = DataReader.get_int(reader, pos++);
        pos = start_date.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
