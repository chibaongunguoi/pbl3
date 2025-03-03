using Microsoft.Data.SqlClient;

class Subject : DataObj
{
    // ========================================================================
    public int id;
    public string name = "";
    public int grade;
    public int duration; // Tính bằng tháng
    public int fee_per_month;
    public int slot_per_week;

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.name = reader.GetString(pos++);
        this.grade = reader.GetInt32(pos++);
        this.duration = reader.GetInt32(pos++);
        this.fee_per_month = reader.GetInt32(pos++);
        this.slot_per_week = reader.GetInt32(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
