using Microsoft.Data.SqlClient;

sealed class Course : IdObj
{
    // ========================================================================
    public int tch_id { get; set; }
    public int sbj_id { get; set; }
    public string name { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        tch_id = DataReader.get_int(reader, ref pos);
        sbj_id = DataReader.get_int(reader, ref pos);
        name = DataReader.get_string(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{tch_id}");
        lst.Add($"{sbj_id}");
        lst.Add(name);
        return lst;
    }

    // ========================================================================
}

/* EOF */
