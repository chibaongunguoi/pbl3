using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string name = "";
    public InfoGender gender = new InfoGender();
    public DateOnly bday = new();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        name = DataReader.get_string(reader, ref pos);
        gender = DataReader.get_enum<InfoGender>(reader, ref pos);
        bday = DataReader.get_date(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(name);
        lst.Add($"{(int)gender}");
        lst.Add(bday.ToString());
        return lst;
    }

    // ========================================================================
}

/* EOF */
