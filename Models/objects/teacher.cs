using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public string thumbnail = "";
    public string description = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        thumbnail = DataReader.get_string(reader, ref pos);
        description = DataReader.get_string(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(thumbnail);
        lst.Add(description);
        return lst;
    }

    // ========================================================================
}

/* EOF */
