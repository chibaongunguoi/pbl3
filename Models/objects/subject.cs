using Microsoft.Data.SqlClient;

class Subject : IdObj
{
    // ========================================================================
    public string name = "";
    public int grade;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        name = DataReader.get_string(reader, ref pos);
        grade = DataReader.get_int(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(name);
        lst.Add($"{grade}");
        return lst;
    }

    // ========================================================================
}

/* EOF */
