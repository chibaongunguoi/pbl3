using Microsoft.Data.SqlClient;

sealed class Semester : IdObj
{
    // ========================================================================
    public int course_id { get; set; }
    public DateOnly start_date { get; set; } = new();
    public DateOnly finish_date { get; set; } = new();
    public int capacity { get; set; }
    public int fee { get; set; }
    public string description { get; set; } = "";
    public string state { get; set; } = "";

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        course_id = DataReader.get_int(reader, ref pos);
        start_date = DataReader.get_date(reader, ref pos);
        finish_date = DataReader.get_date(reader, ref pos);
        capacity = DataReader.get_int(reader, ref pos);
        fee = DataReader.get_int(reader, ref pos);
        description = DataReader.get_string(reader, ref pos);
        state = DataReader.get_string(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{course_id}");
        lst.Add(IoUtils.conv_db(start_date));
        lst.Add(IoUtils.conv_db(finish_date));
        lst.Add($"{capacity}");
        lst.Add($"{fee}");
        lst.Add(description);
        lst.Add(state);
        return lst;
    }

    // ========================================================================
}

/* EOF */
