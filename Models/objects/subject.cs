using Microsoft.Data.SqlClient;

class Subject : IdObj
{
    // ========================================================================
    public string name = "";
    public int grade;
    public int duration; // Tính bằng tháng
    public int fee_per_month;
    public int slot_per_week;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        name = DataReader.get_string(reader, pos++);
        grade = DataReader.get_int(reader, pos++);
        duration = DataReader.get_int(reader, pos++);
        fee_per_month = DataReader.get_int(reader, pos++);
        slot_per_week = DataReader.get_int(reader, pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
