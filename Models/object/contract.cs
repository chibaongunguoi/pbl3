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
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.stu_id = reader.GetInt32(pos++);
        this.tch_id = reader.GetInt32(pos++);
        this.sbj_id = reader.GetInt32(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
