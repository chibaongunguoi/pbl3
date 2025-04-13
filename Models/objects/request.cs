using Microsoft.Data.SqlClient;

sealed class Request : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int semester_id { get; set; }
    public DateOnly date { get; set; } = new();
    public string state { get; set; } = "";

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        stu_id = DataReader.get_int(reader, ref pos);
        semester_id = DataReader.get_int(reader, ref pos);
        date = DataReader.get_date(reader, ref pos);
        state = DataReader.get_string(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{stu_id}");
        lst.Add($"{semester_id}");
        lst.Add(IoUtils.conv_db(date));
        lst.Add(state);
        return lst;
    }

    // ========================================================================
}

/* EOF */
