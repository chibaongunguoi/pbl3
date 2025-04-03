using Microsoft.Data.SqlClient;

sealed class Course : IdObj
{
    // ========================================================================
    public int tch_id { get; set; }
    public int sbj_id { get; set; }
    public string name { get; set; } = "";
    public string description { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        tch_id = DataReader.get_int(reader, pos++);
        sbj_id = DataReader.get_int(reader, pos++);
        name = DataReader.get_string(reader, pos++);
        description = DataReader.get_string(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{tch_id}");
        lst.Add($"{sbj_id}");
        lst.Add(name);
        lst.Add(description);
        return lst;
    }

    // ========================================================================
}

/* EOF */
